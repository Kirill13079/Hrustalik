﻿using HealthApp.API.Data;
using HealthApp.API.Extensions;
using HealthApp.API.Models;
using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using HealthApp.Common.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.API.Controllers.API
{
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserController(
            ApplicationDbContext context,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(ApiRoutes.Register)]
        public async Task<ActionResult> Register([FromBody] Login login)
        {
            if (login == null)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = login.Email,
                Email = login.Email
            };

            var result = await _userManager
                .CreateAsync(user, login.Password);

            if (result.Succeeded)
            {
                var createdUser = await _userManager
                    .FindByEmailAsync(user.Email);

                if (createdUser != null)
                {
                    var customer = new Customer
                    {
                        Email = createdUser.Email
                    };

                    _context.Customers.Add(customer);

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Ok(customer);
                    }
                }
            }

            return BadRequest(result?.Errors?.ToList());
        }

        [AllowAnonymous]
        [HttpPost]
        [Route(ApiRoutes.Login)]
        public async Task<ActionResult> Login([FromBody] Login login)
        {
            if (login == null)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager
                .FindByEmailAsync(login.Email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var isValid = await _userManager
                .CheckPasswordAsync(user, login.Password);

            if (isValid)
            {
                var customer = _context.Customers
                    .FirstOrDefault(x => x.Email == user.Email);

                return Ok(new
                {
                    customer,
                    token = user.GetJwtToken()
                });
            }

            return NotFound("Invalid Credentials");
        }

        [HttpGet(ApiRoutes.GetCustomer)]
        public async Task<ActionResult> GetCustomer()
        {
            var user = await _userManager
                .GetUserAsync(HttpContext.User);
            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == user.Email);

            return Ok(customer);
        }
    }
}
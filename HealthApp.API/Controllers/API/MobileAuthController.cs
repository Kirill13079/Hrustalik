using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.Common;
using HealthApp.Common.Model;
using HealthApp.Common.Model.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.API.Controllers.API
{
    [Route(ApiRoutes.MobileAuth)]
    [ApiController]
    public class MobileAuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public MobileAuthController(ApplicationDbContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{scheme}")]
        public async Task Get([FromRoute] string scheme)
        {
            var auth = await Request.HttpContext.AuthenticateAsync(scheme);

            if (!auth.Succeeded
                || auth?.Principal == null
                || !auth.Principal.Identities.Any(id => id.IsAuthenticated)
                || string.IsNullOrEmpty(auth.Properties.GetTokenValue("access_token")))
            {
                await Request.HttpContext.ChallengeAsync(scheme);
            }
            else
            {
                var claims = auth.Principal.Identities.FirstOrDefault()?.Claims;
                var email = string.Empty;

                email = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (!string.IsNullOrWhiteSpace(email))
                {
                    var user = new User
                    {
                        UserName = email,
                        Email = email
                    };

                    var existingUser = await _userManager.FindByEmailAsync(user.Email);

                    if (existingUser == null)
                    {
                        var result = await _userManager.CreateAsync(user, auth.Properties.GetTokenValue("access_token"));

                        if (result.Succeeded)
                        {
                            var createdUser = await _userManager.FindByEmailAsync(user.Email);

                            if (createdUser != null)
                            {
                                var customer = new Customer
                                {
                                    Email = createdUser.Email
                                };

                                _ = _context.Customers.Add(customer);

                                _ = await _context.SaveChangesAsync();
                            }
                        }
                    }
                }

                var qs = new Dictionary<string, string>
                {
                    { "access_token", auth.Properties.GetTokenValue("access_token") },
                    { "refresh_token", auth.Properties.GetTokenValue("refresh_token") ?? string.Empty },
                    { "expires", (auth.Properties.ExpiresUtc?.ToUnixTimeSeconds() ?? -1).ToString() },
                    { "email", email }
                };

                var url = Constants.CallbackDataSchema + "://#" + string.Join(
                    "&", qs.Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Value != "-1")
                    .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

                Request.HttpContext.Response.Redirect(url);
            }
        }
    }
}

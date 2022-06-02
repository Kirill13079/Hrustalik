using HealthApp.API.Data;
using HealthApp.Common.Model.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.API.Controllers.API
{
    public class CategoriesController : ControllerBase
    {
        private ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route(ApiRoutes.GetCategories)]
        public async Task<IActionResult> GetCategories()
        {
            var arrivals = _context.Categories
                .OrderBy(x => x.Name);

            return Ok(await arrivals.ToListAsync());
        }
    }
}

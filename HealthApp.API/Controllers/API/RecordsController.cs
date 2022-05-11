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
    public class RecordsController : ControllerBase
    {
        private ApplicationDbContext _context;

        public RecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route(ApiRoutes.GetRecords)]
        public async Task<IActionResult> GetNews()
        {
            var arrivals = _context.Records
                    .Where(x => x.DateAdded > DateTimeOffset.Now.AddDays(-30))
                    .Include(x => x.Category);

            return Ok(await arrivals.ToListAsync());
        }
    }
}

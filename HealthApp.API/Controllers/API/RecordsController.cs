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
                    .Where(x => !x.IsHot)
                    .Where(x => !x.IsArticle)
                    .Where(x => !x.IsYoutube)
                    .Include(x => x.Category);

            return Ok(await arrivals.ToListAsync());
        }

        [HttpGet]
        [Route(ApiRoutes.GetHotRecord)]
        public IActionResult GetHotRecord()
        {
            var arrival = _context.Records
                    .Where(x => x.DateAdded > DateTimeOffset.Now.AddDays(-30))
                    .Where(x => x.IsHot)
                    .Include(x => x.Category)
                    .FirstOrDefault();

            return Ok(arrival);
        }

        [HttpGet]
        [Route(ApiRoutes.GetArticleRecords)]
        public async Task<IActionResult> GetArticleRecords()
        {
            var arrivals = _context.Records
                    .Where(x => x.DateAdded > DateTimeOffset.Now.AddDays(-30))
                    .Where(x => x.IsArticle)
                    .Include(x => x.Category);

            return Ok(await arrivals.ToListAsync());
        }

        [HttpGet]
        [Route(ApiRoutes.GetPopularRecords)]
        public IActionResult GetPopularRecords()
        {
            var arrivals = _context.Records
                    .Where(x => x.DateAdded > DateTimeOffset.Now.AddDays(-30))
                    .Where(x => x.IsPopular)
                    .Include(x => x.Category)
                    .Take(5);

            return Ok(arrivals);
        }

        [HttpGet]
        [Route(ApiRoutes.GetYoutubeRecords)]
        public IActionResult GetYoutubeRecords()
        {
            var arrivals = _context.Records
                    .Where(x => x.DateAdded > DateTimeOffset.Now.AddDays(-30))
                    .Where(x => x.IsYoutube)
                    .Include(x => x.Category);

            return Ok(arrivals);
        }
    }
}

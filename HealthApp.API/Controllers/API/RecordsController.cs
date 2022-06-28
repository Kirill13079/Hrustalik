using HealthApp.API.Data;
using HealthApp.Common.Model.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    //.Where(x => x.DateAdded > DateTimeOffset.Now.AddDays(-30))
                    .Where(x => !x.IsHot)
                    .Where(x => !x.IsArticle)
                    //.Where(x => !x.IsYoutube)
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .OrderBy(x => x.DateAdded);

            return Ok(await arrivals.ToListAsync());
        }

        [HttpGet]
        [Route(ApiRoutes.GetHotRecord)]
        public IActionResult GetHotRecord()
        {
            var arrival = _context.Records
                    //.Where(x => x.DateAdded > DateTimeOffset.Now.AddDays(-30))
                    .Where(x => x.IsHot)
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .FirstOrDefault();

            return Ok(arrival);
        }

        [HttpGet]
        [Route(ApiRoutes.GetArticleRecords)]
        public async Task<IActionResult> GetArticleRecords()
        {
            var arrivals = _context.Records
                    //.Where(x => x.DateAdded > DateTimeOffset.Now.AddDays(-30))
                    .Where(x => x.IsArticle)
                    .Include(x => x.Category)
                    .Include(x => x.Author);

            return Ok(await arrivals.ToListAsync());
        }

        [HttpGet]
        [Route(ApiRoutes.GetPopularRecords)]
        public IActionResult GetPopularRecords()
        {
            var arrivals = _context.Records
                    //.Where(x => x.DateAdded > DateTimeOffset.Now.AddDays(-30))
                    .Where(x => x.IsPopular)
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .Take(5);

            return Ok(arrivals);
        }

        [HttpGet]
        [Route(ApiRoutes.GetYoutubeRecords)]
        public IActionResult GetYoutubeRecords()
        {
            var arrivals = _context.Records
                    //.Where(x => x.DateAdded > DateTimeOffset.Now.AddDays(-30))
                    .Where(x => x.IsYoutube)
                    .Include(x => x.Category)
                    .Include(x => x.Author);

            return Ok(arrivals);
        }

        [HttpGet]
        [Route(ApiRoutes.GetCategoryRecords)]
        public IActionResult GetCategoryRecords(int? id)
        {
            var arrivals = _context.Records
                    .Where(x => x.Category.Id == id)
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .OrderBy(x => x.DateAdded);

            return Ok(arrivals);
        }

        [HttpGet]
        [Route(ApiRoutes.GetAuthorRecords)]
        public IActionResult GetAuthorRecords(int? id)
        {
            var arrivals = _context.Records
                    .Where(x => x.Author.Id == id)
                    .Include(x => x.Category)
                    .Include(x => x.Author);

            return Ok(arrivals);
        }
    }
}

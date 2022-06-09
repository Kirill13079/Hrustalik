using HealthApp.API.Data;
using HealthApp.Common.Model.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.API.Controllers.API
{
    public class AuthorsController : ControllerBase
    {
        private ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route(ApiRoutes.GetAuthors)]
        public async Task<IActionResult> GetAuthors()
        {
            var arrivals = _context.Authors
                .OrderBy(x => x.Name);

            return Ok(await arrivals.ToListAsync());
        }
    }
}

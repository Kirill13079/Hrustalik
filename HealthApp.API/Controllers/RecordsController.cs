using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthApp.API.Data;
using HealthApp.Common.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace HealthApp.API.Controllers
{
    [Authorize]
    public class RecordsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public RecordsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Records.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records
                .FirstOrDefaultAsync(m => m.Id == id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Record record, [FromForm] string Category, [FromForm] string Author, IFormFile RecordImage)
        {
            if (ModelState.IsValid)
            {
                string fileName = UploadImage(RecordImage);
                
                var category = _context.Categories
                    .FirstOrDefault(x => x.Id == int.Parse(Category));

                var author = _context.Authors
                    .FirstOrDefault(x => x.Id == int.Parse(Author));

                record.Author = author;
                record.Category = category;
                record.Image = fileName;
                record.DateAdded = DateTimeOffset.Now;

                _context.Add(record);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(record);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records.FindAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Record record, [FromForm] string Category, [FromForm] string Author)
        {
            if (id != record.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var category = _context.Categories
                        .FirstOrDefault(x => x.Id == int.Parse(Category));

                    var author = _context.Authors
                        .FirstOrDefault(x => x.Id == int.Parse(Author));

                    record.Author = author;
                    record.Category = category;
                    record.DateAdded = DateTimeOffset.Now;

                    _context.Update(record);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordExists(record.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(record);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records
                .FirstOrDefaultAsync(m => m.Id == id);

            if (record == null)
            {
                return NotFound();
            }

            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var record = await _context.Records.FindAsync(id);

            _context.Records.Remove(record);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private string UploadImage(IFormFile recordImage)
        {
            string fileName = string.Empty;

            if (recordImage != null)
            {
                var folder = Path.Combine(_environment.WebRootPath, "RecordImages");
                fileName = $"{Guid.NewGuid()}_{recordImage.FileName}";
                var filePatch = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePatch, FileMode.Create))
                {
                    recordImage.CopyTo(stream);
                }
            }

            return fileName;
        }

        private bool RecordExists(int id)
        {
            return _context.Records.Any(e => e.Id == id);
        }
    }
}

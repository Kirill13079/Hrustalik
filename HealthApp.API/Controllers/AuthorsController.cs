using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthApp.API.Data;
using HealthApp.Common.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace HealthApp.API.Controllers
{
    [Authorize]
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AuthorsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Authors.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author, IFormFile AuthorImage)
        {
            if (ModelState.IsValid)
            {
                string fileName = UploadImage(AuthorImage);

                author.Logo = fileName;

                _context.Add(author);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Author author, IFormFile AuthorImage)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (AuthorImage != null)
                    {
                        string fileName = UploadImage(AuthorImage);

                        author.Logo = fileName;
                    }

                    _context.Update(author);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            _context.Authors.Remove(author);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private string UploadImage(IFormFile authorImage)
        {
            string fileName = string.Empty;

            if (authorImage != null)
            {
                var folder = Path.Combine(_environment.WebRootPath, "AuthorImages");
                fileName = $"{Guid.NewGuid()}_{authorImage.FileName}";
                var filePatch = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePatch, FileMode.Create))
                {
                    authorImage.CopyTo(stream);
                }
            }

            return fileName;
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab10.Data;
using Lab10.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Lab10.ViewModels;
using Lab10.Migrations;
using Microsoft.AspNetCore.Authorization;

namespace Lab10.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ArticlesController : Controller
    {
        private readonly StoreDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ArticlesController(StoreDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Articles
       
        public async Task<IActionResult> Index()
        {
            
            var articles = await _context.Articles.Include(a => a.Category).ToListAsync();

            return View(articles);
        }

       
        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }
          
            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                .ToList();
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Image,CategoryId")] ArticleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string imagePath = "";
                if (viewModel.Image != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "upload");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.Image.CopyToAsync(fileStream);
                    }
                    imagePath= Path.Combine("~/upload", uniqueFileName);

                }
                var article = new Article
                {
                    Name = viewModel.Name,
                    Price = viewModel.Price,
                    CategoryId = viewModel.CategoryId,
                    ImagePath = imagePath,
                   
              
            };
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                .ToList();
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Name,Price,CategoryId")] Article article)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingArticle = await _context.Articles.FindAsync(id);

                    if (existingArticle == null)
                    {
                        return NotFound();
                    }

                   
                    existingArticle.Name = article.Name;
                    existingArticle.Price = article.Price;
                    existingArticle.CategoryId = article.CategoryId;

                   

                    _context.Update(existingArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleId))
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
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }
            
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (!string.IsNullOrEmpty(article.ImagePath))
            {
                DeleteImageFile(article.ImagePath);
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void DeleteImageFile(string filePath)
        {
            var fullPath = Path.Combine(_hostingEnvironment.WebRootPath, filePath.TrimStart('~', '/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }


        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}

using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab6.Data;
using lab6.Models;

namespace lab6.Controllers
{
    public class ForumCategorysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumCategorysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ForumCategorys
        public async Task<IActionResult> Index()
        {
            return View(await _context.ForumCategorys.ToListAsync());
        }

        // GET: ForumCategorys/Details
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategorys
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }

            return View(forumCategory);
        }

        // GET: ForumCategorys/Create
        public IActionResult Create()
        {
            return View(new ForumCategory());
        }

        // POST: ForumCategorys/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumCategory model)
        {
            if (ModelState.IsValid)
            {
                _context.ForumCategorys.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }


        // GET: ForumCategorys/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategorys
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }

            return View(forumCategory);
        }

        // POST: ForumCategorys/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, ForumCategory model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategorys
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                forumCategory.Name = model.Name;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: ForumCategorys/Delete
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategorys
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }

            return View(forumCategory);
        }

        // POST: ForumCategorys/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var forumCategory = await _context.ForumCategorys.SingleOrDefaultAsync(m => m.Id == id);
            _context.ForumCategorys.Remove(forumCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DoctorExists(int id)
        {
            return _context.ForumCategorys.Any(e => e.Id == id);
        }
    }
}
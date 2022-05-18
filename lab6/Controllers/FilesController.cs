using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab6.Data;
using lab6.Models;
namespace lab6.Controllers
{
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Files
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Files.Include(f => f.Folder);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Files/Details
        public async Task<IActionResult> Details(Guid? idFile)
        {
            if (idFile == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .Include(f => f.Folder)
                .SingleOrDefaultAsync(m => m.Id == idFile);
            if (file == null)
            {
                return NotFound();
            }
            return View(file);
        }

        // GET: Files/Create
        public async Task<IActionResult> Create(Guid? idParent)
        {
            if (idParent == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders
                .SingleOrDefaultAsync(m => m.Id == idParent);
            if (folder == null)
            {
                return NotFound();
            }

            var folders = await _context.Folders
                .OrderBy(x => x.Name).ToListAsync();
            ViewBag.Folders = folder;
            return View(new FileEditModel());
        }

        // POST: Files/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? idParent, FileEditModel model)
        {
            if (idParent == null)
            {
                NotFound();
            }

            var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.file.ContentDisposition).FileName.Value.Trim('"'));
            var fileExt = Path.GetExtension(fileName);

            var folder = await _context.Folders
                .SingleOrDefaultAsync(m => m.Id == idParent);

            if (folder == null)
            {
                NotFound();
            }

            if (ModelState.IsValid)
            {
                var file = new Models.File
                {
                    FolderId = folder.Id,
                    Name = model.Name,
                    Extension = fileExt,
                    Size = (int)model.file.Length
                };
                byte[] Data = null;

                using (var binaryReader = new BinaryReader(model.file.OpenReadStream()))
                {
                    Data = binaryReader.ReadBytes((int)model.file.Length);
                }
                file.file = Data;

                file.Id = Guid.NewGuid();
                _context.Add(file);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Folders", new { idParent = folder.Id });
            }
            return View(model);
        }

        // GET: Files/Edit
        public async Task<IActionResult> Edit(Guid? idFile)
        {
            if (idFile == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .SingleOrDefaultAsync(m => m.Id == idFile);
            if (file == null)
            {
                return NotFound();
            }
            ViewBag.Files = file;
            return View(file);
        }

        // POST: Files/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid idFile, FileEditModel model)
        {
            if (idFile == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .SingleOrDefaultAsync(m => m.Id == idFile);


            if (file == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                file.Name = model.Name;

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Folders", new { idParent = file.FolderId });
            }
            return View(model);
        }

        // GET: Files/Delete
        public async Task<IActionResult> Delete(Guid? idFile)
        {
            if (idFile == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .Include(f => f.Folder)
                .SingleOrDefaultAsync(m => m.Id == idFile);
            if (file == null)
            {
                return NotFound();
            }

            ViewBag.Files = file;
            return View(file);
        }

        // POST: Files/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid idFile)
        {
            if (idFile == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .SingleOrDefaultAsync(m => m.Id == idFile);
            if (file == null)
            {
                return NotFound();
            }
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Folders", new { idParent = file.FolderId });
        }

        private bool FileExists(Guid id)
        {
            return _context.Files.Any(e => e.Id == id);
        }
    }
}
using EventHorizon.Data;
using EventHorizon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventHorizon.Controllers
{
    public class TeamMembersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamMembersController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: TeamMembers
        public async Task<IActionResult> Index()
        {
            return View(await _context.TeamMembers.ToListAsync());
        }

        // POST: TeamMembers/SetPresident
        [HttpPost]
        public async Task<IActionResult> SetPresident(int memberId)
        {
            // Clear all existing presidents
            var all = await _context.TeamMembers.ToListAsync();
            foreach (var m in all)
                m.IsPresident = false;

            // Set the chosen one (memberId=0 means "no president")
            if (memberId > 0)
            {
                var chosen = all.FirstOrDefault(m => m.Id == memberId);
                if (chosen != null)
                    chosen.IsPresident = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: TeamMembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TeamMembers/Create
        [HttpPost]
        public async Task<IActionResult> Create(TeamMember teamMember, IFormFile? imageFile)
        {
            ModelState.Remove("ImageUrl");

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                    teamMember.ImageUrl = await SaveImageAsync(imageFile);

                _context.TeamMembers.Add(teamMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teamMember);
        }

        // GET: TeamMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null) return NotFound();
            return View(teamMember);
        }

        // POST: TeamMembers/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TeamMember teamMember, IFormFile? imageFile)
        {
            if (id != teamMember.Id) return NotFound();

            ModelState.Remove("ImageUrl");

            if (ModelState.IsValid)
            {
                try
                {
                    // Keep existing IsPresident value from DB (don't let form overwrite it)
                    var existing = await _context.TeamMembers.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    teamMember.IsPresident = existing?.IsPresident ?? false;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        DeleteOldImage(existing?.ImageUrl);
                        teamMember.ImageUrl = await SaveImageAsync(imageFile);
                    }

                    _context.Update(teamMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TeamMembers.Any(e => e.Id == teamMember.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teamMember);
        }

        // GET: TeamMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(m => m.Id == id);
            if (teamMember == null) return NotFound();
            return View(teamMember);
        }

        // POST: TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember != null)
            {
                DeleteOldImage(teamMember.ImageUrl);
                _context.TeamMembers.Remove(teamMember);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ── Helpers ──────────────────────────────────────────────

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var ext = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString() + ext;
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/uploads/" + fileName;
        }

        private void DeleteOldImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl) || !imageUrl.StartsWith("/uploads/"))
                return;
            var fullPath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }
    }
}
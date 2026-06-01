using EventHorizon.Data;
using EventHorizon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventHorizon.Controllers
{
    public class TeamMembersController : Controller
    {
        private readonly AppDbContext _context;

        public TeamMembersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TeamMembers
        public async Task<IActionResult> Index()
        {
            return View(await _context.TeamMembers.ToListAsync());
        }

        // GET: TeamMembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TeamMembers/Create
        [HttpPost]
        public async Task<IActionResult> Create(TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
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
        public async Task<IActionResult> Edit(int id, TeamMember teamMember)
        {
            if (id != teamMember.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
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
                _context.TeamMembers.Remove(teamMember);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
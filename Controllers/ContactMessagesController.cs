using EventHorizon.Data;
using EventHorizon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventHorizon.Controllers
{
    [Authorize]
    public class ContactMessagesController : Controller
    {
        private readonly AppDbContext _context;

        public ContactMessagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ContactMessages
        public async Task<IActionResult> Index()
        {
            // Order by most recent first
            var messages = await _context.ContactMessages
                .OrderByDescending(m => m.SubmittedAt)
                .ToListAsync();
                
            return View(messages);
        }

        // POST: ContactMessages/Submit
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Submit(ContactMessage contactMessage)
        {
            if (ModelState.IsValid)
            {
                contactMessage.SubmittedAt = DateTime.UtcNow;
                _context.ContactMessages.Add(contactMessage);
                await _context.SaveChangesAsync();
                
                // Set a success message via TempData
                TempData["SuccessMessage"] = "Your message has been sent successfully!";
            }
            
            // Redirect back to home page (with the contact anchor)
            return Redirect("/#contact");
        }
        
        // GET: ContactMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var message = await _context.ContactMessages.FirstOrDefaultAsync(m => m.Id == id);
            if (message == null) return NotFound();
            
            return View(message);
        }

        // POST: ContactMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                _context.ContactMessages.Remove(message);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

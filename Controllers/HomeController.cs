using EventHorizon.Data;
using EventHorizon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventHorizon.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new HomeViewModel
            {
                Events = await _context.Events.ToListAsync(),
                TeamMembers = await _context.TeamMembers.ToListAsync()
            };
            return View(vm);
        }
    }
}
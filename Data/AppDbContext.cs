using EventHorizon.Models;
using Microsoft.EntityFrameworkCore;

namespace EventHorizon.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
    }
}
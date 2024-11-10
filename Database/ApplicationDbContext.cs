using Microsoft.EntityFrameworkCore;
using TSNO.Models;

namespace TSNO.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Entity> Entities { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using TSNO.Models;

namespace TSNO.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<StatsEntity> Stats { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StatsEntity>().HasData(
                new StatsEntity { Id = 1, TotalNotes = 0, ActiveNotes = 0 }
            );
        }
    }
}

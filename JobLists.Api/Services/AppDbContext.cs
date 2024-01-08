using JobLists.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobLists.Api.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>().HasKey(x => x.Id);
        }
    }
}

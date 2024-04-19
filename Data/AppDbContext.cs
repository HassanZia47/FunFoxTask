using FunFoxTask.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FunFoxTask.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserToCourse> AppUserToCourse { get; set; }
    }
}

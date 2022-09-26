using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        // EntityFramework-CreateDataTable
        public DbSet<Article> Articles { get; set; }

        public DbSet<AuthUser> AuthUsers { get; set; }
    }
}
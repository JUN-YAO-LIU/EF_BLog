using EFBlog.Models;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace EFBlog.DbAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
    }
}
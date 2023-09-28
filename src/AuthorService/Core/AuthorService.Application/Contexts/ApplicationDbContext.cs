using AuthorService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorService.Application.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Author> Authors => Set<Author>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("User ID = postgres; Password = admin; Server = localhost; Port = 5432; Database = NET2; Integrated Security = true; Pooling = true; Connection Lifetime = 0;",
                    m => { m.EnableRetryOnFailure(); });
            }
        }
    }
}

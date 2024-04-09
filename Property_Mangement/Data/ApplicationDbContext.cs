using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Property_Mangement.Models;

namespace Property_Mangement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Applicationuser> Applicationusers { get; set; }
        public DbSet<Property> Properties { get; set; }
    }
}

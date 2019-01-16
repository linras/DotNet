using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Dogs.Models;

namespace Dogs.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // declare each database entity here to access it with dbContext
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Dog> Dogs { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<DogRace> DogRaces { get; set; }
        public virtual DbSet<Event> Events { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public ApplicationDbContext()
        {
        }
    }
}

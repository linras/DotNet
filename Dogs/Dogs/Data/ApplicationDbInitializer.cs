using System.Linq;
using Dogs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dogs.Data
{
    public class ApplicationDbInitializer
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationDbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public void Seed()
        {
            // create database + apply migrations
            context.Database.Migrate();

            // add example roles
            if (!context.Roles.Any())
            {
                string[] roles = new string[]{
                    "Administrator",
                    "Worker",
                    "Volunteer",
                    "User" };

                foreach (var roleName in roles)
                {
                    var role = new IdentityRole(roleName) { };
                    context.Roles.Add(role);
                }
            }
            
            // add administrator account
            if (!context.ApplicationUsers.Any())
            {
                var user = new ApplicationUser { UserName = "admin@cos.com", Email = "admin@cos.com" };
                userManager.CreateAsync(user, "admin").Wait();
                userManager.AddToRoleAsync(user, "Administrator").Wait();

                user = new ApplicationUser { UserName = "user@cos.com", Email = "user@cos.com" };
                userManager.CreateAsync(user, "user").Wait();
                userManager.AddToRoleAsync(user, "User").Wait();
            }

            // add sample addresses
            if (!context.Addresses.Any()) {

                var address = new Address { };

            }

            context.SaveChanges();
        }
    }
}

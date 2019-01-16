using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Dogs.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //public ICollection<JobOffer> JobOffers { get; set; }
        public ICollection<Dog> Dogs { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}

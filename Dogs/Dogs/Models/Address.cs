using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dogs.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        [RegularExpression(@"[0-9][0-9]-[0-9][0-9][0-9]"), Required, StringLength(6)]
        public string ZipCode { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dogs.Models
{
    public class DogRace
    {
        public int DogRaceId { get; set; }
        public string Race { get; set; }
        public string Description { get; set; }
        public ICollection<Dog> Dogs { get; set; }
    }
}

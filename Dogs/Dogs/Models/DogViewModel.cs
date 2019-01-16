using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dogs.Models
{
    public class DogViewModel : Dog
    {
        [Display(Name = "Dog race")]
        public string DogRaceId { get; set; }
        public int NumberOfEvents { get; set; }
        public IEnumerable<DogRace> DogRaces { get; set; }
    }
}

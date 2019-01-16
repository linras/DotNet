using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dogs.Models
{
    public class Dog : IValidatableObject
    {

        public int DogId { get; set; }
        public string Name { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Remote(
            action: "VerifyDate",
            controller: "Dogs",
            AdditionalFields = "DateOfBirth, HereSince"
            )]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        
        public DateTime HereSince { get; set; }     
        public string Description { get; set; }
        public int DogRaceId { get; set; }
        //public int NumberOfEvents { get; set; }

        [Display(Name = "Race")]
        public DogRace Race { get; set; }
        public ICollection<Event> Events { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {   //default value is today
            if (HereSince.Equals(null))
            {
                HereSince = DateTime.Now;
            }
            else if(HereSince.CompareTo(DateTime.Now) > 0)
            {
                yield return new ValidationResult(
                    $"Dogs cannot check-in later than today.",
                    new[] { "HereSince"});
            }
            //birth date can be unknown
            if (DateOfBirth.Equals(null))
            { }
            //birth date can't be later thay today
            if (DateOfBirth.CompareTo(DateTime.Now) > 0) {
                yield return new ValidationResult(
                    $"birth date can't be later thay today.",
                    new[] { "DateOfBirth" });
            }
            //Impossible for dog to be here faster then it's birth.
            if (DateOfBirth.CompareTo(HereSince) > 0) {
                yield return new ValidationResult(
                    $"Impossible for dog to be here faster then it's birth.",
                    new[] { "HereSince", "DateOfBirth" });
            }
        }
    }


        //public IEnumerable<Race> DogRaces { get; set; }

}


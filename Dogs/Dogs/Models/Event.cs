using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dogs.Models
{
    public class Event : IValidatableObject
    {
        public int EventId { get; set; }
        public ApplicationUser User { get; set; }
        public Dog Doge { get; set; }
        public int DogId { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date)]
        public DateTime When { get; set; }
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {   //if event date is later than today
            if (When.CompareTo(DateTime.Now)<0)
            {
                yield return new ValidationResult(
                    $"You cannot insert events from the past. Today is later then {When}",
                    new[] { "When" });
            }
        }
    }
}

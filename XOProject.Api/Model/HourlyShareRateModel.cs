using System;
using System.ComponentModel.DataAnnotations;

namespace XOProject.Api.Model
{
    public class HourlyShareRateModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        [RegularExpression("^[A-Z]{3}$", ErrorMessage = "Share symbol should be all capital letters with 3 characters")]
        public string Symbol { get; set; }

        [Required]
        [Range(typeof(decimal), "0M", Constants.MaxDecimalValue)]
        public decimal Rate { get; set; }
    }
}

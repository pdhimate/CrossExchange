using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XOProject.Api.ValidationAttributes
{
    public class StringRangeAttribute : ValidationAttribute
    {
        public string[] AllowedValues { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (AllowedValues?.Contains(value?.ToString()) == true)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Allowed values were not found.");
        }
    }
}

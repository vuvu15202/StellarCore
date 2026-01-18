using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stellar.Shared.Utils
{
    public static class ValidatorUtils
    {
        /// <summary>
        /// Validate an object using DataAnnotations.
        /// </summary>
        /// <param name="obj">Object to validate</param>
        /// <returns>List of validation results</returns>
        public static IList<ValidationResult> Validate(object obj)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(obj);
            Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return validationResults;
        }

        /// <summary>
        /// Check if an object is valid according to DataAnnotations.
        /// </summary>
        /// <param name="obj">Object to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            return Validator.TryValidateObject(obj, validationContext, null, true);
        }
    }
}

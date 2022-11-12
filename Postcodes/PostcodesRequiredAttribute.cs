using Postcodes.Models.Request;
using System.ComponentModel.DataAnnotations;

namespace Postcodes
{
    public class PostcodesRequired : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var request = (PostcodesRequest)validationContext.ObjectInstance;

            if (!request?.Postcodes.Any() ?? false)
            {
                return new ValidationResult(ErrorMessages.PostcodesRequired);
            }

            return ValidationResult.Success;
        }
    }
}

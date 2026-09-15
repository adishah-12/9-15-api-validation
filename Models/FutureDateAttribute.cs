using System.ComponentModel.DataAnnotations;

namespace HealthcareApi.Models;

public class FutureDateAttribute : ValidationAttribute
{
    public FutureDateAttribute()
        : base("{0} must be a future date.")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime date && date <= DateTime.Now)
        {
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }
}
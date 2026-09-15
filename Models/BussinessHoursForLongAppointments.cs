using System.ComponentModel.DataAnnotations;

namespace HealthcareApi.Models;

[AttributeUsage(AttributeTargets.Class)]
public class BusinessHoursForLongAppointmentsAttribute : ValidationAttribute
{
    private static readonly TimeSpan BusinessStart = new(9, 0, 0);
    private static readonly TimeSpan BusinessEnd = new(17, 0, 0);
    private const int LongAppointmentThresholdMinutes = 60;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not AppointmentDto appointment)
        {
            return ValidationResult.Success;
        }

        if (appointment.DurationMinutes <= LongAppointmentThresholdMinutes)
        {
            return ValidationResult.Success;
        }

        var startTime = appointment.Date.TimeOfDay;
        if (startTime < BusinessStart || startTime > BusinessEnd)
        {
            return new ValidationResult(
                $"Appointments longer than {LongAppointmentThresholdMinutes} minutes must be booked between 9am and 5pm.",
                new[] { nameof(AppointmentDto.Date) });
        }

        return ValidationResult.Success;
    }
}
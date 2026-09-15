using System.ComponentModel.DataAnnotations;

namespace HealthcareApi.Models;

[BusinessHoursForLongAppointments]
public class AppointmentDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string PatientName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [FutureDate]
    public DateTime Date { get; set; }

    [Required]
    [Range(15, 120)]
    public int DurationMinutes { get; set; }
}
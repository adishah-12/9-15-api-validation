using HealthcareApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private static readonly List<AppointmentDto> Appointments = new();

    [HttpGet]
    public ActionResult<IEnumerable<AppointmentDto>> GetAppointments()
    {
        return Ok(Appointments);
    }

    [HttpPost]
    public ActionResult<AppointmentDto> CreateAppointment(AppointmentDto appointment)
    {
        // TODO: Add ModelState.IsValid check here
        
        // Assign a simple ID (this is bad practice - just for demo)
        appointment.Id = Appointments.Count + 1;
        Appointments.Add(appointment);
        
        // Return the appointment with the assigned ID
        return Ok(appointment);
    }
}
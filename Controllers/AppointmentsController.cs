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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Assign a simple ID (bad)
        appointment.Id = Appointments.Count + 1;
        Appointments.Add(appointment);
        
        // Return the appointment with the assigned ID
        return Ok(appointment);
    }
}
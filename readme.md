# Lab 1: HealthcareApi Validation

```bash
dotnet run
```

Swagger UI opens at `http://localhost:5000/swagger`.

## Endpoints

All in `Controllers/AppointmentsController.cs`.

* `GET /api/appointments`: all appointments, 200
* `POST /api/appointments`: validates via `ModelState.IsValid`, 400 with error dictionary on failure, 200 with assigned `Id` on success

## Validation

* Built-in attributes: `Models/AppointmentDto.cs`, `[Required]`/`[StringLength(100)]` on `PatientName`, `[Required]`/`[EmailAddress]` on `Email`, `[Required]`/`[Range(15, 120)]` on `DurationMinutes`
* `ModelState.IsValid` check: `Controllers/AppointmentsController.cs`, `CreateAppointment`
* Property-level custom attribute: `Models/FutureDateAttribute.cs`, applied to `Date`
* Class-level custom attribute: `Models/BusinessHoursForLongAppointmentsAttribute.cs`, applied to `AppointmentDto`, rejects appointments over 60 minutes booked outside 9am–5pm

## Curl requests for Manual Tests

```bash
curl http://localhost:5000/api/appointments

curl -i -X POST http://localhost:5000/api/appointments -H "Content-Type: application/json" -d '{"patientName":"Jane Doe","email":"jane@example.com","date":"2026-10-01T10:00:00","durationMinutes":30}'

curl -i -X POST http://localhost:5000/api/appointments -H "Content-Type: application/json" -d '{"patientName":"","email":"jane@example.com","date":"2026-10-01T10:00:00","durationMinutes":30}'

curl -i -X POST http://localhost:5000/api/appointments -H "Content-Type: application/json" -d '{"patientName":"Jane Doe","email":"not-an-email","date":"2026-10-01T10:00:00","durationMinutes":30}'

curl -i -X POST http://localhost:5000/api/appointments -H "Content-Type: application/json" -d '{"patientName":"Jane Doe","email":"jane@example.com","date":"2025-01-01T10:00:00","durationMinutes":30}'

curl -i -X POST http://localhost:5000/api/appointments -H "Content-Type: application/json" -d '{"patientName":"Jane Doe","email":"jane@example.com","date":"2026-10-01T10:00:00","durationMinutes":999}'

curl -i -X POST http://localhost:5000/api/appointments -H "Content-Type: application/json" -d '{"patientName":"Jane Doe","email":"jane@example.com","date":"2026-10-01T08:00:00","durationMinutes":90}'

curl -i -X POST http://localhost:5000/api/appointments -H "Content-Type: application/json" -d '{"patientName":"Jane Doe","email":"jane@example.com","date":"2026-10-01T10:00:00","durationMinutes":90}'
```

| Case | Result |
|---|---|
| GET all | 200 |
| POST valid, 30 min | 200, appointment added with assigned `Id` |
| POST missing `PatientName` | 400, error on `PatientName` |
| POST invalid email | 400, error on `Email` |
| POST past date | 400, `FutureDate` error on `Date` |
| POST `DurationMinutes = 999` | 400, `Range` error on `DurationMinutes` |
| POST 90 min at 8am | 400, `BusinessHoursForLongAppointments` error on `Date` |
| POST 90 min at 10am | 200, appointment added |

## Quick Reflection

**Easiest to implement:** Built-in data annotations (`AppointmentDto.cs`). Declarative, framework wires them into `ModelState` automatically. `FutureDateAttribute.cs` took more work: inherit `ValidationAttribute`, override `IsValid`, write the comparison by hand.

**FluentValidation:** Rules move to a separate `AppointmentDtoValidator` class instead of attributes on the model. Easier to unit test in isolation, easier to swap rules per environment without touching the DTO.

**Why it matters for API security:** Validation runs at model binding, before `CreateAppointment` executes. Bad durations, malformed emails, and past dates never reach the in-memory store. Before this lab, `ModelState.IsValid` was never checked, so any payload was accepted as-is.
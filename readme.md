# Lab 1: Implementing API Validation

- Duration: ~1 hour
- Context: You are consultants working with a state healthcare provider. They want a Web API where patients can book appointments, but they are especially concerned about bad or malicious data being sent into their system. Your job is to implement validation on the API’s DTOs to ensure clean, safe input.

---

#### Learning Objectives

* Decorate DTO properties with built-in validation attributes (`[Required]`, `[StringLength]`, `[Range]`) to enforce common data constraints.
* Check the `ModelState.IsValid` property within controller actions to programmatically handle validation failures and return appropriate 400 Bad Request responses.
* Implement a custom validation attribute by inheriting from `ValidationAttribute` and overriding the `IsValid` method to contain complex business logic.
* Develop a class-level validation attribute to perform validation that depends on multiple properties of a single model.
* Compare and contrast the use of data annotation attributes versus a library like FluentValidation for managing complex rule sets.
* Analyze how robust model validation serves as a primary defense against common security vulnerabilities like mass assignment attacks.


---

#### Starter Project

You are given a simple ASP.NET Core Web API project:

```
/HealthcareApi
  /Controllers
    AppointmentsController.cs   // TODO: Add endpoints with validation
  /Models
    AppointmentDto.cs           // TODO: Add validation attributes
  Program.cs                    // Preconfigured with minimal API + Swagger
  README.md
```

The API has an in-memory list of appointments but currently does not validate inputs.

---

#### Tasks

#### 1. Test Current Behavior (5 min)

Goal: See what happens when we send invalid data to an API without validation.

* Run the project and open Swagger UI
* Try to POST an appointment with invalid data:
  * Missing `PatientName` (send empty string or null)
  * Invalid email format (e.g., "not-an-email")
  * Past date (e.g., yesterday)
  * Invalid duration (e.g., 999 minutes or -5)
* Observe what happens:
  * Does the API accept the bad data? (It should!)
  * What status code do you get? (Probably 200 OK)
  * Does the appointment get added to the list?
  * What problems could this cause for the healthcare system?

Checkpoint: What problems do you see with the current API behavior? Why is this dangerous for a healthcare system?

---

#### 2. Add Built-In Validation Attributes (15 min)

Open `AppointmentDto.cs` and enforce common constraints using data annotations:

* `PatientName` → required, max length 100
* `Email` → required, valid email format
* `Date` → required, must be a future date (custom logic later)
* `DurationMinutes` → required, between 15 and 120

👉 Look up “ASP.NET Core data annotations” for the exact attribute names.

Checkpoint: Try POSTing invalid data in Swagger (e.g. missing name). Does the framework automatically reject it?

---

#### 3. Check `ModelState.IsValid` (10 min)

In `AppointmentsController.cs`, update the POST action:

* Before processing, check `ModelState.IsValid`.
* If invalid, return 400 Bad Request with the model state errors.

👉 Hint: Search “ModelState.IsValid ASP.NET Core Web API example”.

Checkpoint: Send an appointment with `DurationMinutes = 999`. Do you see a 400 error?

---

#### 4. Custom Property-Level Validation (15 min)

Create a custom attribute `FutureDateAttribute`:

* Inherit from `ValidationAttribute`.
* Override `IsValid`.
* Ensure the appointment date is greater than `DateTime.Now`.

Apply it to the `Date` property in `AppointmentDto`.

Checkpoint: Test with past and future dates. Do you get the right validation errors?

---

#### 5. Custom Class-Level Validation (10 min)

Sometimes validation depends on multiple properties. Add a new rule:

* Appointments longer than 60 minutes can only be booked between 9am and 5pm.
* Implement this as a class-level attribute on `AppointmentDto`.

👉 Hint: Create an attribute that implements `IValidatableObject` or a class-level `ValidationAttribute`.

Checkpoint: Try posting a 90-minute appointment at 8am. Do you see an error?

---

#### 6. Reflection: Data Annotations vs FluentValidation (5 min)

In the PR description, reflect on:

* What’s good about data annotations (quick, built-in, close to the model)?
* What limitations might push you to FluentValidation (complex rules, externalised logic)?

You don’t have to implement FluentValidation now — just think critically.

---

#### Stretch Goals

* Implement FluentValidation for `AppointmentDto` and compare error messages.
* Add a global filter that automatically returns validation errors without needing `ModelState.IsValid` in each controller.
* Research “mass assignment attacks” and add notes on how validation helps mitigate them.

---

#### Deliverables

* `AppointmentDto` decorated with validation attributes.
* `AppointmentsController` POST action checking `ModelState.IsValid`.
* A working custom property-level attribute (`FutureDateAttribute`).
* A class-level attribute validating multiple properties.
* A pull request with reflective answers to:

  1. Which validation approach was easiest to implement?
  2. How could FluentValidation improve rule management?
  3. Why is robust validation important for API security?
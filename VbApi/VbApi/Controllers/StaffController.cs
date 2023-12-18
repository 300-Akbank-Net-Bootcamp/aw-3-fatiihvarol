using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace VbApi.Controllers;

public class Staff
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public decimal? HourlySalary { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class StaffController : ControllerBase
{
        private readonly IValidator<Staff> _staffValidator;

     public StaffController(IValidator<Staff> staffValidator)
    {
        _staffValidator = staffValidator ?? throw new ArgumentNullException(nameof(staffValidator));
    }

    [HttpPost]
    public IActionResult Post([FromBody] Staff value)
    {
        var validationResult = _staffValidator.Validate(value);
       if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
        }

       
        return Ok(value);
    }
}

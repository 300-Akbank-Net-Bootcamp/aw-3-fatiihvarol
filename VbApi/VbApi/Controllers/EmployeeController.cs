using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace VbApi.Controllers;

public class Employee 
{
    public string? Name { get; set; }
  
    public DateTime DateOfBirth { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

   
    public double HourlySalary { get; set; }


}



[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IValidator<Employee> _employeeValidator;

    public EmployeeController(IValidator<Employee> employeeValidator)
    {
        _employeeValidator = employeeValidator ?? throw new ArgumentNullException(nameof(employeeValidator));
    }

    [HttpPost]
    public IActionResult Post([FromBody] Employee value)
    {
        var validationResult = _employeeValidator.Validate(value);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage));
        }

       
        return Ok(value);
    }
}

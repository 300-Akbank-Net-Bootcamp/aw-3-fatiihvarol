using FluentValidation;
using VbApi.Controllers;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth cannot be empty.")
            .Must(BeAValidDate).WithMessage("Invalid date of birth.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number cannot be empty.")
            .Length(10, 20).WithMessage("Phone number must be between 10 and 20 characters.");

            RuleFor(x => x.HourlySalary)
            .NotEmpty().WithMessage("Hourly salary cannot be empty.")
            .Must(BeAValidSalary).WithMessage("Hourly salary must be between 30 and 500.");

    }

    private bool BeAValidSalary(double HourlySalary)
    {
           return HourlySalary >= 30 && HourlySalary <= 500;
    }

    // Yardımcı metot: Geçerli bir tarih kontrolü
    private bool BeAValidDate(DateTime date)
    {
       return date<DateTime.Now && date>DateTime.Now.AddYears(-100);
    }
   
}

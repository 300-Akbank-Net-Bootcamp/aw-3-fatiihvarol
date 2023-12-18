using FluentValidation;
using VbApi.Controllers;

public class StaffValidator : AbstractValidator<Staff>
{
    public StaffValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");


        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number cannot be empty.")
            .Length(10, 20).WithMessage("Phone number must be between 10 and 20 characters.");

        RuleFor(x => x.HourlySalary)
            .NotEmpty().WithMessage("Hourly salary cannot be empty.")
            .GreaterThanOrEqualTo(0).WithMessage("Hourly salary must be a non-negative value.");
    }

    // Yardımcı metot: Geçerli bir tarih kontrolü
    private static bool BeAValidDate(DateTime date)
    {
        return date < DateTime.Now;
    }
}

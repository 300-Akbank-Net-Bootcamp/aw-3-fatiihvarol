using FluentValidation;
using VbApi.Schema.DTO;

namespace VbApi.Business.Validator;

public class CreateContactValidator:AbstractValidator<ContactRequest>
{
    public CreateContactValidator()
    {
        RuleFor(x => x.ContactType).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Information).NotEmpty().MaximumLength(50);
        
    }
}
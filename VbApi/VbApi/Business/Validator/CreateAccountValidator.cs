using FluentValidation;
using VbApi.Schema.DTO;

namespace VbApi.Business.Validator;

public class CreateAccountValidator:AbstractValidator<AccountRequest>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.Balance).InclusiveBetween(0, 1000000);
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.CurrencyType).NotEmpty().MaximumLength(10);
    }
}
using FluentValidation;
using VbApi.Schema.DTO;

namespace VbApi.Business.Validator;

public class CreateAddressValidator : AbstractValidator<AddressRequest>
{
    public CreateAddressValidator()
    {
        RuleFor(x => x.Address1).MinimumLength(10).MaximumLength(100);
        RuleFor(x=>x.Address2).MinimumLength(10).MaximumLength(100);
        RuleFor(x => x.PostalCode).Length(5);
        RuleFor(x=>x.City).MinimumLength(2).MaximumLength(50);
        RuleFor(x=>x.Country).MinimumLength(2).MaximumLength(50);
        RuleFor(x=>x.County).MinimumLength(2).MaximumLength(50);

        
    }
}
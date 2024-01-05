using FluentValidation;
using VbApi.Schema.DTO;

namespace VbApi.Business.Validator;

public class CreateEftTransactionValidator:AbstractValidator<EftTransactionRequest>

{
    public CreateEftTransactionValidator()
    {
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.SenderIban).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Amount).InclusiveBetween(0, 9999999);
        RuleFor(x => x.SenderName).NotEmpty();
        RuleFor(x => x.SenderAccount).NotEmpty();
        
    }
}
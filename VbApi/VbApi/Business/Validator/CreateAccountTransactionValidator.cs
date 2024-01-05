using System.Data;
using FluentValidation;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Validator;

public class CreateAccountTransactionValidator:AbstractValidator<AccountTransactionRequest>
{
    public CreateAccountTransactionValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Amount).InclusiveBetween(0, 9999999);
        RuleFor(x => x.TransferType).NotEmpty();
        RuleFor(x => x.AccountId).NotEmpty();
        
    }
}
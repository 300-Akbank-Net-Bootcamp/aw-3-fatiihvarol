using MediatR;
using Vb.Base.Response;
using VbApi.Schema.DTO;

namespace VbApi.Business.Cqrs;

public record CreateAccountCommand(AccountRequest Model) : IRequest<ApiResponse<AccountResponse>>;
public record UpdateAccountCommand(int Id,AccountRequest Model) : IRequest<ApiResponse>;
public record DeleteAccountCommand(int Id) : IRequest<ApiResponse>;

    
    
public record GetAllAccountsQuery() : IRequest<ApiResponse<List<AccountResponse>>>;
public record GetAccountByIdQuery(int Id) : IRequest<ApiResponse<AccountResponse>>;
public record GetAccountsByParameterQuery(string CurrencyType, string IBAN, string AccountNumber)
    : IRequest<ApiResponse<List<AccountResponse>>>;
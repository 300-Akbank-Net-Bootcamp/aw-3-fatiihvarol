using MediatR;
using Vb.Base.Response;
using VbApi.Schema.DTO;

namespace VbApi.Business.Cqrs;

public record CreateAccountTransactionCommand(AccountTransactionRequest Model) : IRequest<ApiResponse<AccountTransactionResponse>>;
public record UpdateAccountTransactionCommand(int Id,string Description) : IRequest<ApiResponse>;
public record DeleteAccountTransactionCommand(int Id) : IRequest<ApiResponse>;

    
    
public record GetAllAccountTransactionsQuery() : IRequest<ApiResponse<List<AccountTransactionResponse>>>;
public record GetAccountTransactionById(int Id) : IRequest<ApiResponse<AccountTransactionResponse>>;
public record GetAccountTransactionsByParameter(decimal Amount, int AccountId)
    : IRequest<ApiResponse<List<AccountTransactionResponse>>>;
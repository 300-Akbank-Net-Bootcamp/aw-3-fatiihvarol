using MediatR;
using Vb.Base.Response;
using VbApi.Schema.DTO;

namespace VbApi.Business.Cqrs;

public record CreateEftTransactionCommand(EftTransactionRequest Model) : IRequest<ApiResponse<EftTransactionResponse>>;
public record UpdateEftTransactionCommand(int Id, string Description) : IRequest<ApiResponse>;
public record DeleteEftTransactionCommand(int Id):IRequest<ApiResponse>;

public record GetAllEftTransactionsQuery():IRequest<ApiResponse<List<EftTransactionResponse>>>;
public record GetEftTransactionByIdQuery(int Id):IRequest<ApiResponse<EftTransactionResponse>>;
public record GetEftTransactionsByParameterQuery(string SenderName,string SenderIban,decimal Amount):IRequest<ApiResponse<List<EftTransactionResponse>>>;
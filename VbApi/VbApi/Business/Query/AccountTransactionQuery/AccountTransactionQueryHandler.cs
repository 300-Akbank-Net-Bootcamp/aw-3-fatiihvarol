using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Query.AccountTransactionQuery;

public class AccountTransactionQueryHandler:
    IRequestHandler<GetAllAccountTransactionsQuery,ApiResponse<List<AccountTransactionResponse>>>,
    IRequestHandler<GetAccountTransactionById,ApiResponse<AccountTransactionResponse>>,
    IRequestHandler<GetAccountTransactionsByParameter,ApiResponse<List<AccountTransactionResponse>>>
{
    private readonly VbDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AccountTransactionQueryHandler(VbDbContext dbContext, IMediator mediator, IMapper mapper)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<AccountTransactionResponse>>> Handle(GetAllAccountTransactionsQuery request, CancellationToken cancellationToken)
    {
        var accountTransactions =await _dbContext.Set<AccountTransaction>().ToListAsync(cancellationToken);
        if (accountTransactions.Count == 0)
            return new ApiResponse<List<AccountTransactionResponse>>("not found any accountTransaction");
        var mapped = _mapper.Map<List<AccountTransaction>, List<AccountTransactionResponse>>(accountTransactions);
        return new ApiResponse<List<AccountTransactionResponse>>(mapped);

    }

    public async Task<ApiResponse<AccountTransactionResponse>> Handle(GetAccountTransactionById request, CancellationToken cancellationToken)
    {
        var accountTransaction = await _dbContext.Set<AccountTransaction>()
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (accountTransaction is null)
        {
            return new ApiResponse<AccountTransactionResponse>("not found");
        }

        var mappedResponse = _mapper.Map<AccountTransaction,AccountTransactionResponse>(accountTransaction);

        return new ApiResponse<AccountTransactionResponse>(mappedResponse);
    }

    public async Task<ApiResponse<List<AccountTransactionResponse>>> Handle(GetAccountTransactionsByParameter request, CancellationToken cancellationToken)
    {
        var accountTransactions = await _dbContext.Set<AccountTransaction>()
            .Where(x => x.Amount == request.Amount || x.AccountId == request.AccountId )
            .ToListAsync(cancellationToken);
        if (accountTransactions.Count == 0)
            return new ApiResponse<List<AccountTransactionResponse>>("not found");
        
        var mappedAccount = _mapper.Map<List<AccountTransaction>, List<AccountTransactionResponse>>(accountTransactions);
        return new ApiResponse<List<AccountTransactionResponse>>(mappedAccount);
    }
}
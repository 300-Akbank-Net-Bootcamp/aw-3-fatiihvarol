using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Command.AccountTransactionCommand;

public class AccountTransactionCommandHandler :
    IRequestHandler<CreateAccountTransactionCommand, ApiResponse<AccountTransactionResponse>>,
    IRequestHandler<UpdateAccountTransactionCommand, ApiResponse>,
    IRequestHandler<DeleteAccountTransactionCommand, ApiResponse>
{
    private readonly VbDbContext _dbContext;
    private readonly IMapper _mapper;

    public AccountTransactionCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<AccountTransactionResponse>> Handle(
        CreateAccountTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var account = await _dbContext.Set<Account>().FirstOrDefaultAsync(x => x.Id == request.Model.AccountId, cancellationToken);

        if (account == null)
        {
            return new ApiResponse<AccountTransactionResponse>("Account not found");
        }

        // Check account balance is enough for account transaction !
        if (account.Balance < request.Model.Amount)
            return new ApiResponse<AccountTransactionResponse>("Insufficient balance");
        

        var mapped = _mapper.Map<AccountTransactionRequest, AccountTransaction>(request.Model);
        _dbContext.Add(mapped);

        // Update the account balance
        account.Balance -= request.Model.Amount;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<AccountTransaction, AccountTransactionResponse>(mapped);
        return new ApiResponse<AccountTransactionResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var accountTransaction = await _dbContext.Set<AccountTransaction>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (accountTransaction is null)
            return new ApiResponse("accountTransaction not found ");
        accountTransaction.Description = request.Description;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var accountTransaction = await _dbContext.Set<AccountTransaction>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (accountTransaction is null)
            return new ApiResponse("accountTransaction not found ");
        accountTransaction.IsActive = false;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}
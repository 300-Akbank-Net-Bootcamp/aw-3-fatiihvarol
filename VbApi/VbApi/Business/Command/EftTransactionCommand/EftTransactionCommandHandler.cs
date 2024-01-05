using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Command.EftTransactionCommand;

public class EftTransactionCommandHandler:
    IRequestHandler<CreateEftTransactionCommand,ApiResponse<EftTransactionResponse>>,
    IRequestHandler<UpdateEftTransactionCommand,ApiResponse>,
    IRequestHandler<DeleteEftTransactionCommand,ApiResponse>


{
    private readonly VbDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public EftTransactionCommandHandler(VbDbContext dbContext, IMediator mediator,IMapper mapper)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ApiResponse<EftTransactionResponse>> Handle(CreateEftTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Set<Account>().FirstOrDefaultAsync(x => x.Id == request.Model.AccountId, cancellationToken);

        if (account == null)
        {
            return new ApiResponse<EftTransactionResponse>("Account not found");
        }

        // Check account balance is enough for eft !
        if (account.Balance < request.Model.Amount)
        {
            return new ApiResponse<EftTransactionResponse>("Insufficient balance");
        }

        var mapped = _mapper.Map<EftTransactionRequest, EftTransaction>(request.Model);
        _dbContext.Add(mapped);

        // Update the account balance
        account.Balance -= request.Model.Amount;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<EftTransaction, EftTransactionResponse>(mapped);
        return new ApiResponse<EftTransactionResponse>(response);
    }



    public async  Task<ApiResponse> Handle(UpdateEftTransactionCommand request, CancellationToken cancellationToken)
    {
        var eftTransaction = await _dbContext.Set<EftTransaction>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (eftTransaction is null)
            return new ApiResponse("Eft Not Found");
        eftTransaction.Description = request.Description;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteEftTransactionCommand request, CancellationToken cancellationToken)
    {
        var eftTransaction = await _dbContext.Set<EftTransaction>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
       if (eftTransaction is null)
           return new ApiResponse("Eft Not Found");

       eftTransaction.IsActive = false;
      await _dbContext.SaveChangesAsync(cancellationToken);
      return new ApiResponse();
    }
}
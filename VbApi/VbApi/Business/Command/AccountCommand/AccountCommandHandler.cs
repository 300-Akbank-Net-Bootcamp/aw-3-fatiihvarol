using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Command.AccountCommand
{
    public class AccountCommandHandler :
        IRequestHandler<CreateAccountCommand, ApiResponse<AccountResponse>>,
        IRequestHandler<UpdateAccountCommand, ApiResponse>,
        IRequestHandler<DeleteAccountCommand, ApiResponse>
    {
        private readonly VbDbContext _dbContext;
        private readonly IMapper _mapper;

        public AccountCommandHandler(VbDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<AccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            // Check if user exists based on the provided CustomerId
            var user = await _dbContext.Set<Customer>().FirstOrDefaultAsync(x => x.Id == request.Model.CustomerId, cancellationToken);

            if (user is null)
                return new ApiResponse<AccountResponse>("User Not Found");

            var mappedAccount = _mapper.Map<AccountRequest, Account>(request.Model);
            mappedAccount.AccountNumber = GenerateNewAccountNumber();
            _dbContext.Add(mappedAccount);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<Account, AccountResponse>(mappedAccount);

            return new ApiResponse<AccountResponse>(response);
        }

        public async Task<ApiResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Set<Account>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            
            if (account is null)
                return new ApiResponse("Account not found");
            var accountNumber = account.AccountNumber;
            // Map the properties from the updated request to the existing entity
            _mapper.Map(request.Model, account);
            account.AccountNumber = accountNumber;
            
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ApiResponse("Updated");
        }

        public async Task<ApiResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Set<Account>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (account is null)
                return new ApiResponse("Account Not Found");

            account.IsActive = false;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ApiResponse("Deleted");
        }
        private static int GenerateNewAccountNumber()
        {
            return new Random().Next(1000000, 9999999);
        }
        private async Task<int> GenerateUniqueAccountNumberAsync(CancellationToken cancellationToken)
        {
            int newAccountNumber;
            do
            {
                newAccountNumber = GenerateNewAccountNumber();
            } while (await _dbContext.Set<Account>().AnyAsync(x => x.AccountNumber == newAccountNumber, cancellationToken));

            return newAccountNumber;
        }
    }
}

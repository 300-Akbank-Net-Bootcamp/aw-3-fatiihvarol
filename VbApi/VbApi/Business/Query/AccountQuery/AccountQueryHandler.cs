using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Query.AccountQuery
{
    public class AccountQueryHandler :
        IRequestHandler<GetAllAccountsQuery, ApiResponse<List<AccountResponse>>>,
        IRequestHandler<GetAccountByIdQuery, ApiResponse<AccountResponse>>,
        IRequestHandler<GetAccountsByParameterQuery, ApiResponse<List<AccountResponse>>>
    {
        private readonly VbDbContext _dbContext;
        private readonly IMapper _mapper;

        public AccountQueryHandler(VbDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<AccountResponse>>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _dbContext.Set<Account>()
                .ToListAsync(cancellationToken);

            var mappedAccounts = _mapper.Map<List<Account>, List<AccountResponse>>(accounts);

            return new ApiResponse<List<AccountResponse>>(mappedAccounts);
        }

        public async Task<ApiResponse<AccountResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Set<Account>()
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (account is null)
                return new ApiResponse<AccountResponse>("not found");

            var mappedAccount = _mapper.Map<Account, AccountResponse>(account);
            return new ApiResponse<AccountResponse>(mappedAccount);
        }

        public async Task<ApiResponse<List<AccountResponse>>> Handle(GetAccountsByParameterQuery request, CancellationToken cancellationToken)
        {
            var accounts = await _dbContext.Set<Account>()
                .Where(x => x.AccountNumber.ToString() == request.AccountNumber|| x.IBAN == request.IBAN||x.CurrencyType==request.CurrencyType)
                .ToListAsync(cancellationToken);

            if (accounts.Count == 0)
                return new ApiResponse<List<AccountResponse>>("not found");

            var mappedAccounts = _mapper.Map<List<Account>, List<AccountResponse>>(accounts);
            return new ApiResponse<List<AccountResponse>>(mappedAccounts);
        }
    }
}

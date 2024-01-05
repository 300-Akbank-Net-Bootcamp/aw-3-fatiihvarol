using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Query.EftTransactionQuery
{
    public class EftTransactionQueryHandler :
        IRequestHandler<GetAllEftTransactionsQuery, ApiResponse<List<EftTransactionResponse>>>,
        IRequestHandler<GetEftTransactionByIdQuery, ApiResponse<EftTransactionResponse>>,
        IRequestHandler<GetEftTransactionsByParameterQuery, ApiResponse<List<EftTransactionResponse>>>
    {
        private readonly VbDbContext _dbContext;
        private readonly IMapper _mapper;

        public EftTransactionQueryHandler(VbDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<EftTransactionResponse>>> Handle(GetAllEftTransactionsQuery request, CancellationToken cancellationToken)
        {
            var eftTransactions = await _dbContext.Set<EftTransaction>()
                .ToListAsync(cancellationToken);

            var mappedEftTransactions = _mapper.Map<List<EftTransaction>, List<EftTransactionResponse>>(eftTransactions);

            return new ApiResponse<List<EftTransactionResponse>>(mappedEftTransactions);
        }

        public async Task<ApiResponse<EftTransactionResponse>> Handle(GetEftTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var eftTransaction = await _dbContext.Set<EftTransaction>()
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (eftTransaction is null)
                return new ApiResponse<EftTransactionResponse>("not found");

            var mappedEftTransaction = _mapper.Map<EftTransaction, EftTransactionResponse>(eftTransaction);
            return new ApiResponse<EftTransactionResponse>(mappedEftTransaction);
        }

        public async Task<ApiResponse<List<EftTransactionResponse>>> Handle(GetEftTransactionsByParameterQuery request, CancellationToken cancellationToken)
        {
            var eftTransactions = await _dbContext.Set<EftTransaction>()
                .Where(x => x.SenderName == request.SenderName||x.SenderIban==request.SenderIban||x.Amount==request.Amount) // Adjust this according to your EftTransaction entity
                .ToListAsync(cancellationToken);

            if (eftTransactions.Count == 0)
                return new ApiResponse<List<EftTransactionResponse>>("not found");

            var mappedEftTransactions = _mapper.Map<List<EftTransaction>, List<EftTransactionResponse>>(eftTransactions);
            return new ApiResponse<List<EftTransactionResponse>>(mappedEftTransactions);
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
    using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Query.CustomerQuery;

public class CustomerQueryHandler:
    IRequestHandler<GetAllCustomersQuery,ApiResponse<List<CustomerResponse>>>,
    IRequestHandler<GetCustomerByIdQuery, ApiResponse<CustomerResponse>>,
    IRequestHandler<GetCustomerByParameterQuery, ApiResponse<List<CustomerResponse>>>


{
    private readonly VbDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public CustomerQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<CustomerResponse>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<Customer>()
            .Include(x => x.Accounts)
            .Include(x => x.Addresses)
            .Include(x => x.Contacts)
            .ToListAsync(cancellationToken);
        var mappedList = _mapper.Map<List<Customer>, List<CustomerResponse>>(list);
        return new ApiResponse<List<CustomerResponse>>(mappedList);

    }

    public async Task<ApiResponse<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var entity =  await _dbContext.Set<Customer>()
            .Include(x => x.Accounts)
            .Include(x => x.Contacts)
            .Include(x => x.Addresses)
            .FirstOrDefaultAsync(x => x.CustomerNumber == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<CustomerResponse>("Record not found");
        }
        
        var mapped = _mapper.Map<Customer, CustomerResponse>(entity);
        return new ApiResponse<CustomerResponse>(mapped);
        
    }

    public async Task<ApiResponse<List<CustomerResponse>>> Handle(GetCustomerByParameterQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.Set<Customer>()
            .Include(x => x.Accounts)
            .Include(x => x.Contacts)
            .Include(x => x.Addresses)
            .Where(x =>
                x.FirstName.Contains(request.FirstName) ||
                x.LastName.Contains(request.LastName) ||
                x.IdentityNumber.Contains(request.IdentityNumber)
            ).ToListAsync(cancellationToken);


        if ( !list.Any())
        {
            return new ApiResponse<List<CustomerResponse>>("No matching records found");
        }


        var mappedList = _mapper.Map<List<Customer>, List<CustomerResponse>>(list);
        return new ApiResponse<List<CustomerResponse>>(mappedList);

    }
}



using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Query.AddressQuery;

public class AddressQueryHandler :
    IRequestHandler<GetAllAddressesQuery, ApiResponse<List<AddressResponse>>>,
    IRequestHandler<GetAddressById, ApiResponse<AddressResponse>>,
    IRequestHandler<GetAddressByParameter, ApiResponse<List<AddressResponse>>>
{
    private readonly VbDbContext _dbContext;
    private readonly IMapper _mapper;

    public AddressQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<AddressResponse>>> Handle(GetAllAddressesQuery request,
        CancellationToken cancellationToken)
    {
        var addresses = await _dbContext.Set<Address>()
            .ToListAsync(cancellationToken);

        var mappedAddresses = _mapper.Map<List<Address>, List<AddressResponse>>(addresses);

        return new ApiResponse<List<AddressResponse>>(mappedAddresses);
    }


    public async Task<ApiResponse<AddressResponse>> Handle(GetAddressById request, CancellationToken cancellationToken)
    {
        var address = await _dbContext.Set<Address>()
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (address is null)
            return new ApiResponse<AddressResponse>("not found");

        var mappedAddress = _mapper.Map<Address, AddressResponse>(address);
        return new ApiResponse<AddressResponse>(mappedAddress);
    }

    public async Task<ApiResponse<List<AddressResponse>>> Handle(GetAddressByParameter request,
        CancellationToken cancellationToken)
    {
        var addresses = await _dbContext.Set<Address>()
            .Where(x => x.Address1 == request.Address1 || x.Address2 == request.Address2 || x.City == request.City)
            .ToListAsync(cancellationToken);
        if (addresses.Count == 0)
            return new ApiResponse<List<AddressResponse>>("not found");
        var mappedAddresses = _mapper.Map<List<Address>, List<AddressResponse>>(addresses);
        return new ApiResponse<List<AddressResponse>>(mappedAddresses);
    }
}
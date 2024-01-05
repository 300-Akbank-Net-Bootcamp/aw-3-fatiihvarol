using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Command.AddressCommand;

public class AddressCommandHandler:
    IRequestHandler<CreateAddressCommand,ApiResponse<AddressResponse>>,
    IRequestHandler<UpdateAddressCommand,ApiResponse>,
    IRequestHandler<DeleteAddressCommand,ApiResponse>


{
    private readonly VbDbContext _dbContext;
    private readonly IMapper _mapper;

    public AddressCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<AddressResponse>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        // Check if user exists based on the provided CustomerId
        var user = await _dbContext.Set<Customer>().FirstOrDefaultAsync(x => x.Id == request.Model.CustomerId, cancellationToken);

        if (user is null)
            return new ApiResponse<AddressResponse>("User Not Found");

        var mapped = _mapper.Map<AddressRequest, Address>(request.Model);
        
        _dbContext.Add(mapped);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
         var response = _mapper.Map<AddressRequest, AddressResponse>(request.Model);

        return new ApiResponse<AddressResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _dbContext.Set<Address>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (address is null)
            return new ApiResponse("Address not found");

        // Map properties from request.Model to the existing address entity
        address.Address1 = request.Model.Address1;
        address.Address2 = request.Model.Address2;
        address.Country = request.Model.Country;
        address.City = request.Model.City;
        address.County = request.Model.County;
        address.PostalCode = request.Model.PostalCode;
        address.IsDefault = request.Model.IsDefault;

        _dbContext.Update(address);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }




    public async Task<ApiResponse> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _dbContext.Set<Address>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (address is null)
            return new ApiResponse("Address Not Found");
        address.IsActive = false;
        await  _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();

    }
}
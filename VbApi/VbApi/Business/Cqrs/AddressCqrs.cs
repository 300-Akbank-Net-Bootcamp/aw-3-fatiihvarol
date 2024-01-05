using MediatR;
using Vb.Base.Response;
using VbApi.Schema.DTO;

namespace VbApi.Business.Cqrs;

    public record CreateAddressCommand(AddressRequest Model) : IRequest<ApiResponse<AddressResponse>>;
    public record UpdateAddressCommand(int Id,AddressRequest Model) : IRequest<ApiResponse>;
    public record DeleteAddressCommand(int Id) : IRequest<ApiResponse>;

    
    
    public record GetAllAddressesQuery() : IRequest<ApiResponse<List<AddressResponse>>>;
    public record GetAddressById(int Id) : IRequest<ApiResponse<AddressResponse>>;
    public record GetAddressByParameter(string Address1, string Address2, string City)
        : IRequest<ApiResponse<List<AddressResponse>>>;


using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Schema.DTO;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly VbDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AddressesController(VbDbContext dbContext,IMapper mapper, IMediator mediator)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<AddressResponse>>> GetAllAddresses()
    {
        var operation = new GetAllAddressesQuery();
        var result = await _mediator.Send(operation);
        return result;
    }
    [HttpGet("{Id}")]
    public async Task<ApiResponse<AddressResponse>> GetAddressById(int Id)
    {
        var operation = new GetAddressById(Id);
        var result = await _mediator.Send(operation);
        return result;
    }
    [HttpGet("Parameter")]
    public async Task<ApiResponse<List<AddressResponse>>> GetAddressesByParameter(string Address1,string Address2,string City)
    {
        var operation = new GetAddressByParameter(Address1,Address2,City);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<AddressResponse>> CreateAddress([FromBody] AddressRequest newAddress)
    {
        var operation = new CreateAddressCommand(newAddress);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> UpdateAddress(int id, [FromBody] AddressRequest newAddress)
    {
        var operation = new UpdateAddressCommand(id, newAddress);
        var result =await _mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteAddress(int id)
    {
        var operation = new DeleteAddressCommand(id);
        var result = await _mediator.Send(operation);
        return result;
    }
}
using Microsoft.AspNetCore.Mvc;


using AutoMapper;
using MediatR;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Schema.DTO;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CustomersController : ControllerBase
{
    private readonly VbDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CustomersController(VbDbContext dbContext,IMapper mapper, IMediator mediator)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<CustomerResponse>>> GetAllCustomer()
    {
        var operation = new GetAllCustomersQuery();
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<CustomerResponse>> GetCustomerById(int id)
    {
        var operation = new GetCustomerByIdQuery(id);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpGet("search")]

    public async Task<ApiResponse<List<CustomerResponse>>> GetCustomerByParameter(string firstName, string lastName, string identityNumber)
    {
        var operation = new GetCustomerByParameterQuery(firstName, lastName, identityNumber);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<CustomerResponse>> CreateCustomer([FromBody] CustomerRequest newCustomer)
    {
        var operation = new CreateCustomerCommand(newCustomer);
        var result = await _mediator.Send(operation);
        return result;
    }
    [HttpPut]
    public async Task<ApiResponse> UpdateCustomer(int id,[FromBody] CustomerRequest newCustomer)
    {
        var operation = new UpdateCustomerCommand(id,newCustomer);
        var result = await _mediator.Send(operation);
        return result;
    }
    [HttpDelete]
    public async Task<ApiResponse> DeleteCustomer(int id)
    {
        var operation = new DeleteCustomerCommand(id);
        var result = await _mediator.Send(operation);
        return result;
    }


    
}

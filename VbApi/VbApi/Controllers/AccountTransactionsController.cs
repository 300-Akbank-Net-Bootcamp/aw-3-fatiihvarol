using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountTransactionsController
{
    private readonly VbDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AccountTransactionsController(VbDbContext dbContext,IMapper mapper, IMediator mediator)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ApiResponse<List<AccountTransactionResponse>>> GetAllAccountTransactions()
    {
        var operation = new GetAllAccountTransactionsQuery();
        var result = await _mediator.Send(operation);
        return result;
    }
    [HttpGet("{Id}")]
    public async Task<ApiResponse<AccountTransactionResponse>> GetAccountTransactionById(int Id)
    {
        var operation = new GetAccountTransactionById(Id);
        var result = await _mediator.Send(operation);
        return result;
    }
    [HttpGet("Parameter")]
    public async Task<ApiResponse<List<AccountTransactionResponse>>> GetAccountTransactionsByParameter(decimal amount,int accounId)
    {
        var operation = new GetAccountTransactionsByParameter(amount,accounId);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<AccountTransactionResponse>> CreateAccountTransaction([FromBody] AccountTransactionRequest transaction)
    {
        var operation = new CreateAccountTransactionCommand(transaction);
        var result = await _mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> UpdateAccountTransaction(int id, string description)
    {
        var operation = new UpdateAccountTransactionCommand(id, description);
        var result =await _mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteAccountTransaction(int id)
    {
        var operation = new DeleteAccountTransactionCommand(id);
        var result = await _mediator.Send(operation);
        return result;
    }
}
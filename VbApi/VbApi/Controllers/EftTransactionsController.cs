using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Schema.DTO;

namespace VbApi.Controllers;
[ApiController]
[Route("api/[controller]")]

public class EftTransactionsController
{
     private readonly VbDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public EftTransactionsController(VbDbContext dbContext, IMapper mapper, IMediator mediator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<List<EftTransactionResponse>>> GetAllEftTransaction()
        {
            var operation = new GetAllEftTransactionsQuery();
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<EftTransactionResponse>> GetContactById(int id)
        {
            var operation = new GetEftTransactionByIdQuery(id);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpGet("Parameter")]
        public async Task<ApiResponse<List<EftTransactionResponse>>> GetContactsByParameter(string senderName , string senderIban,decimal amount)
        {
            var operation = new GetEftTransactionsByParameterQuery( senderName,senderIban,amount);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<EftTransactionResponse>> CreateContact([FromBody] EftTransactionRequest newContact)
        {
            var operation = new CreateEftTransactionCommand(newContact);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse> UpdateContact(int id, string description)
        {
            var operation = new UpdateEftTransactionCommand(id, description);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteContact(int id)
        {
            var operation = new DeleteEftTransactionCommand(id);
            var result = await _mediator.Send(operation);
            return result;
        }
}
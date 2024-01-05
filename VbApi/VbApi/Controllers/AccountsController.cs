using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Schema.DTO;

namespace VbApi.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AccountsController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<List<AccountResponse>>> GetAllAccounts()
        {
            var operation = new GetAllAccountsQuery();
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpGet("{Id}")]
        public async Task<ApiResponse<AccountResponse>> GetAccountById(int Id)
        {
            var operation = new GetAccountByIdQuery(Id);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpGet("Parameter")]
        public async Task<ApiResponse<List<AccountResponse>>> GetAccountsByParameter(string accountNumber, string IBAN,string CurrencyType)
        {
            var operation = new GetAccountsByParameterQuery(CurrencyType,IBAN,accountNumber);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<AccountResponse>> CreateAccount([FromBody] AccountRequest newAccount)
        {
            var operation = new CreateAccountCommand(newAccount);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse> UpdateAccount(int id, [FromBody] AccountRequest updatedAccount)
        {
            var operation = new UpdateAccountCommand(id, updatedAccount);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteAccount(int id)
        {
            var operation = new DeleteAccountCommand(id);
            var result = await _mediator.Send(operation);
            return result;
        }
    }


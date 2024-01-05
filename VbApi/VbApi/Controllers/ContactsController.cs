using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Schema.DTO;

namespace VbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly VbDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ContactsController(VbDbContext dbContext, IMapper mapper, IMediator mediator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<List<ContactResponse>>> GetAllContacts()
        {
            var operation = new GetAllContactsQuery();
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<ContactResponse>> GetContactById(int id)
        {
            var operation = new GetContactByIdQuery(id);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpGet("Parameter")]
        public async Task<ApiResponse<List<ContactResponse>>> GetContactsByParameter(string contactType , string information)
        {
            var operation = new GetContactsByParameterQuery( contactType,information);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<ContactResponse>> CreateContact([FromBody] ContactRequest newContact)
        {
            var operation = new CreateContactCommand(newContact);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse> UpdateContact(int id, [FromBody] ContactRequest updatedContact)
        {
            var operation = new UpdateContactCommand(id, updatedContact);
            var result = await _mediator.Send(operation);
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse> DeleteContact(int id)
        {
            var operation = new DeleteContactCommand(id);
            var result = await _mediator.Send(operation);
            return result;
        }
    }
}

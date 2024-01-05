using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Query.ContactQuery
{
    public class ContactQueryHandler :
        IRequestHandler<GetAllContactsQuery, ApiResponse<List<ContactResponse>>>,
        IRequestHandler<GetContactByIdQuery, ApiResponse<ContactResponse>>,
        IRequestHandler<GetContactsByParameterQuery, ApiResponse<List<ContactResponse>>>
    {
        private readonly VbDbContext _dbContext;
        private readonly IMapper _mapper;

        public ContactQueryHandler(VbDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<ContactResponse>>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _dbContext.Set<Contact>()
                .ToListAsync(cancellationToken);

            var mappedContacts = _mapper.Map<List<Contact>, List<ContactResponse>>(contacts);

            return new ApiResponse<List<ContactResponse>>(mappedContacts);
        }

        public async Task<ApiResponse<ContactResponse>> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            var contact = await _dbContext.Set<Contact>()
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (contact is null)
                return new ApiResponse<ContactResponse>("not found");

            var mappedContact = _mapper.Map<Contact, ContactResponse>(contact);
            return new ApiResponse<ContactResponse>(mappedContact);
        }

        public async Task<ApiResponse<List<ContactResponse>>> Handle(GetContactsByParameterQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _dbContext.Set<Contact>()
                .Where(x => x.Information == request.Information || x.ContactType == request.ContactType )
                .ToListAsync(cancellationToken);

            if (contacts.Count == 0)
                return new ApiResponse<List<ContactResponse>>("not found");

            var mappedContacts = _mapper.Map<List<Contact>, List<ContactResponse>>(contacts);
            return new ApiResponse<List<ContactResponse>>(mappedContacts);
        }
    }
}

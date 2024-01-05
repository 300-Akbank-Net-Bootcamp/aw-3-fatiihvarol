using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Command.ContactCommand
{
    public class ContactCommandHandler :
        IRequestHandler<CreateContactCommand, ApiResponse<ContactResponse>>,
        IRequestHandler<UpdateContactCommand, ApiResponse>,
        IRequestHandler<DeleteContactCommand, ApiResponse>
    {
        private readonly VbDbContext _dbContext;
        private readonly IMapper _mapper;

        public ContactCommandHandler(VbDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ContactResponse>> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            // Check if user exists based on the provided CustomerId
            var user = await _dbContext.Set<Customer>().FirstOrDefaultAsync(x => x.Id == request.Model.CustomerId, cancellationToken);

            if (user is null)
                return new ApiResponse<ContactResponse>("User Not Found");

            var mapped = _mapper.Map<ContactRequest, Contact>(request.Model);

            _dbContext.Add(mapped);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<ContactRequest, ContactResponse>(request.Model);

            return new ApiResponse<ContactResponse>(response);
        }

        public async Task<ApiResponse> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _dbContext.Set<Contact>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (contact is null)
                return new ApiResponse("Contact not found");

            // Map the properties from the updated request to the existing entity
            _mapper.Map(request.Model, contact);

            // Set the Id property separately
          

            // Mark the entity as modified
            _dbContext.Entry(contact).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ApiResponse();
        }



        public async Task<ApiResponse> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _dbContext.Set<Contact>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (contact is null)
                return new ApiResponse("Contact Not Found");

            contact.IsActive = false;
           
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ApiResponse();
        }
    }
}

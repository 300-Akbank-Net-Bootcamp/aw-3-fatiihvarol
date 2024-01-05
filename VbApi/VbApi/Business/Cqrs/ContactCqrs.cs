using MediatR;
using Vb.Base.Response;
using VbApi.Schema.DTO;

namespace VbApi.Business.Cqrs;

public record CreateContactCommand(ContactRequest Model) : IRequest<ApiResponse<ContactResponse>>;
public record UpdateContactCommand(int Id,ContactRequest Model) : IRequest<ApiResponse>;
public record DeleteContactCommand(int Id):IRequest<ApiResponse>;

public record GetAllContactsQuery():IRequest<ApiResponse<List<ContactResponse>>>;
public record GetContactByIdQuery(int Id):IRequest<ApiResponse<ContactResponse>>;
public record GetContactsByParameterQuery(string ContactType,string Information):IRequest<ApiResponse<List<ContactResponse>>>;
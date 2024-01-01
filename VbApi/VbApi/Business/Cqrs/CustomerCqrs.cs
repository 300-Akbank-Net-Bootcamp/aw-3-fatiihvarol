using MediatR;
using Vb.Base.Response;
using VbApi.Schema.DTO;

namespace VbApi.Business.Cqrs;

public record CreateCustomerCommand(CustomerRequest Model) : IRequest<ApiResponse<CustomerResponse>>;
public record UpdateCustomerCommand(int Id,CustomerRequest Model) :IRequest<ApiResponse>;
public record DeleteCustomerCommand(int Id):IRequest<ApiResponse>;


public record GetAllCustomersQuery():IRequest<ApiResponse<List<CustomerResponse>>>;
public record GetCustomerByIdQuery(int Id):IRequest<ApiResponse<CustomerResponse>>;
public record GetCustomerByParameterQuery(string FirstName,string LastName,string IdentityNumber):IRequest<ApiResponse<List<CustomerResponse>>>;

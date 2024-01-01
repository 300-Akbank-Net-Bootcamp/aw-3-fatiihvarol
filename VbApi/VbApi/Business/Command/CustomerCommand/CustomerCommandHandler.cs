using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using VbApi.Business.Cqrs;
using VbApi.Data.Entity;
using VbApi.Schema.DTO;

namespace VbApi.Business.Command.CustomerCommand;

public class CustomerCommandHandler :
    IRequestHandler<CreateCustomerCommand, ApiResponse<CustomerResponse>>,
    IRequestHandler<UpdateCustomerCommand, ApiResponse>,
    IRequestHandler<DeleteCustomerCommand, ApiResponse>


{
    private readonly VbDbContext _dbContext;
    private readonly IMapper _mapper;

    public CustomerCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CustomerResponse>> Handle(CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        // IdentityNumber kontrolü
        var existingCustomer = await _dbContext.Set<Customer>()
            .FirstOrDefaultAsync(x => x.IdentityNumber == request.Model.IdentityNumber);

        if (existingCustomer != null)
        {
            return new ApiResponse<CustomerResponse>("Identity Number already taken");
        }


        var newCustomerNumber = await GenerateUniqueCustomerNumberAsync(cancellationToken);

        var entity = _mapper.Map<CustomerRequest, Customer>(request.Model);
        entity.CustomerNumber = newCustomerNumber;

        foreach (var account in entity.Accounts)
        {
            account.AccountNumber = await GenerateUniqueAccountNumberAsync(cancellationToken);
        }


        _dbContext.Set<Customer>().Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var customerResponse = _mapper.Map<Customer, CustomerResponse>(entity);

        return new ApiResponse<CustomerResponse>(customerResponse);
    }

    // CustomerNumber oluşturma ve kontrolü
    private async Task<int> GenerateUniqueCustomerNumberAsync(CancellationToken cancellationToken)
    {
        int newCustomerNumber;
        do
        {
            newCustomerNumber = GenerateNewCustomerNumber();
        } while (await _dbContext.Set<Customer>()
                     .AnyAsync(x => x.CustomerNumber == newCustomerNumber, cancellationToken));

        return newCustomerNumber;
    }

    // Hesap numarası kontrolü (unique)
    private async Task<int> GenerateUniqueAccountNumberAsync(CancellationToken cancellationToken)
    {
        int newAccountNumber;
        do
        {
            newAccountNumber = GenerateNewAccountNumber();
        } while (await _dbContext.Set<Account>().AnyAsync(x => x.AccountNumber == newAccountNumber, cancellationToken));

        return newAccountNumber;
    }

    private static int GenerateNewCustomerNumber()
    {
        return new Random().Next(1000000, 9999999);
    }

    private static int GenerateNewAccountNumber()
    {
        return new Random().Next(1000000, 9999999);
    }

    public async Task<ApiResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await _dbContext.Set<Customer>().Where(x => x.CustomerNumber == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (fromdb is null)
        {
            return new ApiResponse("Record not found");
        }

        fromdb.FirstName = request.Model.FirstName;
        fromdb.LastName = request.Model.LastName;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await _dbContext.Set<Customer>().Where(x => x.CustomerNumber == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (fromdb is null)
        {
            return new ApiResponse("User not found");
        }

        fromdb.IsActive = false;
        var result = _dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}
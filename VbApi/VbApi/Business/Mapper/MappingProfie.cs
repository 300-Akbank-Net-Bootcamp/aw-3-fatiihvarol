using AutoMapper;
using VbApi.Schema.DTO;

using VbApi.Data.Entity;

namespace VbApi.Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerRequest, Customer>();
            CreateMap<Customer, CustomerResponse>();
        
            CreateMap<AddressRequest, Address>();
            CreateMap<Address, AddressResponse>()
                .ForMember(dest => dest.CustomerName,
                    src => src.MapFrom(x => x.Customer.FirstName + " " + x.Customer.LastName));

        
            CreateMap<ContactRequest, Contact>();
            CreateMap<Contact, ContactResponse>()
                .ForMember(dest => dest.CustomerName,
                    src => src.MapFrom(x => x.Customer.FirstName + " " + x.Customer.LastName));
        
            CreateMap<AccountRequest, Account>();
            CreateMap<Account, AccountResponse>()
                .ForMember(dest => dest.CustomerName,
                    src => src.MapFrom(x => x.Customer.FirstName + " " + x.Customer.LastName));

        
            CreateMap<AccountTransactionRequest, AccountTransaction>();
            CreateMap<AccountTransaction, AccountTransactionResponse>();
        
            CreateMap<EftTransactionRequest, EftTransaction>();
            CreateMap<EftTransaction, EftTransactionResponse>();


        }
    }
}
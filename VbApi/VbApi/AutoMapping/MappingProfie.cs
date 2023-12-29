using AutoMapper;
using VbApi.DTO;
using VbApi.Entity;

namespace VbApi.AutoMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerDTO, Customer>()
                .PreserveReferences();

            CreateMap<AddressDTO, Address>()
                .PreserveReferences();

            CreateMap<ContactDTO, Contact>()
                .PreserveReferences();

            CreateMap<AccountDTO, Account>()
                .PreserveReferences();

            CreateMap<Customer, CustomerDTO>()
                .PreserveReferences();
            CreateMap<EftTransaction, EftTransactionDTO>()
                .PreserveReferences();
            CreateMap<AccountTransaction, AccountTransactionDTO>()
                .PreserveReferences();
            
            CreateMap<Account, AccountDTO>()
                .PreserveReferences();
            
            CreateMap<Address, AddressDTO>()
                .PreserveReferences();
            CreateMap<Contact, ContactDTO>()
                .PreserveReferences();
        
            CreateMap<EftTransactionDTO, EftTransaction>()
                .PreserveReferences();
            CreateMap<AccountTransactionDTO, AccountTransaction>()
                .PreserveReferences();
          
        }
    }
}
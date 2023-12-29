    using VbApi.DTO;
    using VbApi.Entity;

    namespace VbApi.DTO;

    public class CustomerDTO
    {
       
        public string IdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CustomerNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastActivityDate { get; set; }

        public List<AddressDTO>? AddressDtos { get; set; } 
        public List<ContactDTO>? ContactDtos { get; set; } 
        public List<AccountDTO>? AccountDtos { get; set; } 

       
    }

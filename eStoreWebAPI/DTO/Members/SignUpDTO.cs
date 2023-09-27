using System.ComponentModel.DataAnnotations;

namespace eStoreWebAPI.DTO.Members
{
    public class SignUpDTO
    {
       
        public string Email { get; set; }

        public string CompanyName { get; set; }
 
        public string Country { get; set; }
 
        public string City { get; set; }
  
        public string Password { get; set; }
    }
}

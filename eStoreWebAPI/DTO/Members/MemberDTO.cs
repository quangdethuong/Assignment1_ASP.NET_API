using System.ComponentModel.DataAnnotations;

namespace eStoreWebAPI.DTO.Members
{
    public class MemberDTO
    {
        public int MemberId { get; set; }

        [Required(ErrorMessage ="Email DTO")]
        public string Email { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

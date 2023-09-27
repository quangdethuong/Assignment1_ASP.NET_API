using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }
        [Required(ErrorMessage ="Email required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "CompanyName required")]

        public string CompanyName { get; set; }
        [Required(ErrorMessage = "City required")]

        public string City { get; set; }
        [Required(ErrorMessage = "Country required")]

        public string Country { get; set; }

        [Required(ErrorMessage = "Password required")]

        public string Password { get; set; }
        public virtual ICollection<Order> orders { get; set; }
        public Member()
        {
            orders = new HashSet<Order>();
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System;

namespace eStoreWebAPI.DTO.Order
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime RequiredDate { get; set; }
        [Required]
        public DateTime ShippedDate { get; set; }
        [Required]
        public decimal Freight { get; set; }
    }
}

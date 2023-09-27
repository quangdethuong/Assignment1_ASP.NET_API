using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category Name")]
        public int? CategoryId { get; set; }

        [Display(Name = "Product Name")]

        [Required(ErrorMessage = "ProductName is required hihiihi")]
        [MaxLength(100)]
        public string ProductName { get; set; }
        [Display(Name = "Weight (gram)")]

        public double Weight { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Unit Price ($)")]

        public decimal UnitPrice { get; set; }
        [Required]
        [Display(Name = "Unit In Stock")]
        public int UnitsInStock { get; set; }

        [ForeignKey("CategoryId")]
        public Category category { get; set; }

        public virtual ICollection<OrderDetail> orderDetails { get; set; }


        //public Product()
        //{
        //    orderDetails = new HashSet<OrderDetail>();
        //}


    }
}

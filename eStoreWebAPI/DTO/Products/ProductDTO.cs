using BusinessObject;
using System.ComponentModel.DataAnnotations;

namespace eStoreWebAPI.DTO.Products
{
    public class ProductDTO
    {

        public int ProductId { get; set; }
    
        public int CategoryId { get; set; }
 
        public string ProductName { get; set; }
      
        public double Weight { get; set; }
  
        public decimal UnitPrice { get; set; }

        public decimal UnitsInStock { get; set; }

        public Category category { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace eStoreWebAPI.DTO.Products
{
    public class ProductCreateDTO
    {
      

        public int CategoryId { get; set; }


   
        public string ProductName { get; set; }

        public double Weight { get; set; }

        public decimal UnitPrice { get; set; }
        
        public decimal UnitsInStock { get; set; }

    }
}

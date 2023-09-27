using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Products
{
    public class ProductRepository : IProductRepository
    {
        public void DeleteProduct(Product p) => ProductDAO.DeleteProduct(p);


        public List<Category> GetCategories() => CategoryDAO.GetCategories();


        public Product GetProductById(int id) => ProductDAO.FindProductById(id);


        public List<Product> GetProducts() => ProductDAO.GetProduct();

      
        public void SaveProduct(Product p) => ProductDAO.SaveProduct(p);
        public List<Product> SearchProduct(string search) => ProductDAO.SearchProduct(search);



        public void UpdateProduct(Product p) => ProductDAO.UpdateProduct(p);
    }
}

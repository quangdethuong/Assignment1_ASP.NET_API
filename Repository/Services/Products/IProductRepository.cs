using BusinessObject;
using System;
using System.Collections.Generic;

namespace Repository.Services.Products
{
    public interface IProductRepository
    {
        void SaveProduct(Product p);
        Product GetProductById(int id);
        void DeleteProduct(Product p);
        void UpdateProduct(Product p);
        List<Category> GetCategories();
        List<Product> GetProducts();
        List<Product> SearchProduct(string search);

    }
}

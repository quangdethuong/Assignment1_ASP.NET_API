using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        public static List<Product> GetProduct()
        {
            var listProduct = new List<Product>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listProduct = context.Products.Include(p => p.category).ToList();
                    Console.WriteLine(listProduct);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listProduct;
        }

        public static Product FindProductById(int proId)
        {
            Product p = new Product();
            try
            {
                using (var context = new MyDbContext())
                {
                    p = context.Products.SingleOrDefault(x => x.ProductId == proId);
                    if (p != null)
                    {
                        var e = context.Entry(p);
                        e.Collection(c => c.orderDetails).Load();
                        e.Reference(p => p.category).Load();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return p;
        }


        public static void SaveProduct(Product p)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Products.Add(p);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void UpdateProduct(Product p)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<Product>(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void DeleteProduct(Product p)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.Products.SingleOrDefault(c => c.ProductId == p.ProductId);
                    context.Products.Remove(p1);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static List<Product> SearchProduct(string search)
        {
            List<Product> products = new List<Product>();

            try
            {
                using (var context = new MyDbContext())
                {
                    products = context.Products.Where(p => string.IsNullOrEmpty(p.ProductName) ||
                                                        p.ProductName.Contains(search.ToLower()) ||
                                                        p.UnitPrice.ToString().Contains(search.ToLower())).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return products;
        }
    }
}

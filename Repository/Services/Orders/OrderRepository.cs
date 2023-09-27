using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Orders
{
    public class OrderRepository : IOrderRepository
    {
        public void DeleteOrder(Order o) => OrderDAO.DeleteOrder(o);

        public Order GetOrderById(int id) => OrderDAO.FindOrderById(id);

        public List<Order> GetOrders() => OrderDAO.GetOrders();

        public void SaveOrder(Order o) => OrderDAO.SaveOrder(o);

        public void UpdateOrder(Order o) => OrderDAO.UpdateOrder(o);

        public List<Order> Filter(DateTime startDate, DateTime endDate) => OrderDAO.Filter(startDate, endDate);
    }
}

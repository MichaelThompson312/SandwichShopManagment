using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;


namespace LogicLayer
{
    public interface IOrderManager
    {
        int AddOrder(Order order, User user);
                
        List<Order> GetOrderByStatus(string status);
        List<Order> GetAllActiveOrders();
        bool UpdateOrderStatus(string status, int orderID);

        bool CreateOrderItem(int standardItemID, int orderID);
    }
}

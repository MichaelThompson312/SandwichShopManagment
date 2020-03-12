using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;


namespace DataAccess
{
    public interface IOrderAccessor
    {
        int insertOrder(Order order, User user);

        List<Order> RetrieveAllOrdersByStatus(string status);
        List<Order> RetrieveAllActiveOrders();
        bool UpdateOrderStatus(string status, int orderID);
        bool InsertOrderItem(int standardItemID, int orderID);
    }
}

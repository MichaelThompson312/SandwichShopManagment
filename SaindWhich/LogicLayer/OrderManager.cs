using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccess;
using System.Security.Cryptography;

namespace LogicLayer
{
    public class OrderManager : IOrderManager
    {
        private IOrderAccessor _orderAccessor;

        public OrderManager()
        {
            _orderAccessor = new OrderAccessor();
        }

        public OrderManager(IOrderAccessor orderAccessor)
        {
            _orderAccessor = orderAccessor;
        }

        public int AddOrder(Order order, User user)
        {
            int result = 0;

            try
            {
                result = _orderAccessor.insertOrder(order, user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Add Failed", ex);
            }
            return result;
        }

        public List<Order> GetOrderByStatus(string status)
        {
            try
            {
                return _orderAccessor.RetrieveAllOrdersByStatus(status);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("List Not Avalible", ex);
            }
        }

        
        public bool CreateOrderItem(int standardItemID, int orderID)
        {
            bool result = false;

            try
            {
                result = (_orderAccessor.InsertOrderItem(standardItemID, orderID));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Adding OrderItem Failed", ex);
            }
            return result;
        }

        public bool UpdateOrderStatus(string status, int orderID)
        {
            bool result = false;

            try
            {
                result = (_orderAccessor.UpdateOrderStatus(status, orderID));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Add Failed", ex);
            }
            return result;
        }

        public List<Order> GetAllActiveOrders()
        {
            try
            {
                return _orderAccessor.RetrieveAllActiveOrders();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("List Not Avalible", ex);
            }
        }
    }
}

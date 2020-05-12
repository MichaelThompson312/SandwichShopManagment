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

        public OrderManager(IOrderAccessor orderAccessor)
        {
            _orderAccessor = orderAccessor;
        }

        public OrderManager()
        {
            _orderAccessor = new OrderAccessor();
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

        public Order GetOrderByID(int id)
        {
            try
            {
                return _orderAccessor.RetrieveOrderById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Order Not Avalible", ex);
            }
        }

        public Order CreateRandomOrder(int customerID, int numberOfItems)
        {
            Order order = new Order();

            for (int i = 0; i < numberOfItems - 1; i++)
            {
                Random random = new Random();

                //return random.Next(min, max);
            }

            return order;
        }

        public List<Order> GetOrderByEmail(string email)
        {
            try
            {
                return _orderAccessor.RetrieveOrderByEmail(email);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("List Not Avalible", ex);
            }
        }

        public List<Order> GetOrderByEmailAndActive(string email)
        {
            try
            {
                return _orderAccessor.RetrieveOrderByEmailAndActive(email);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("List Not Avalible", ex);
            }
        }
    }
}

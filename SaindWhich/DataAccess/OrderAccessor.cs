using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DataObjects;

namespace DataAccess
{
    public class OrderAccessor : IOrderAccessor
    {

        public int insertOrder(Order order, User user)
        {
            int OrderID = 0;
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_insert_order", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            int employeeID = user.EmployeeID;
            cmd.Parameters["@EmployeeID"].Value = employeeID;

            try
            {
                conn.Open();
                order.OrderID = Convert.ToInt32(cmd.ExecuteScalar());
                OrderID = order.OrderID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return OrderID;
        }

        public bool InsertOrderItem(int standardItemID, int orderID)
        {
            bool result = false;

            // connection
            var conn = DBConnection.GetConnection();

            // command
            var cmd = new SqlCommand("sp_insert_orderitem");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@OrderID", SqlDbType.Int);
            cmd.Parameters.Add("@StandardItemID", SqlDbType.Int);
            

            // values
            cmd.Parameters["@OrderID"].Value = orderID;
            cmd.Parameters["@StandardItemID"].Value = standardItemID;

            // execute
            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                result = (rowsAffected == 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<Order> RetrieveAllActiveOrders()
        {
            List<Order> orders = new List<Order>();

            var conn = DBConnection.GetConnection();

            var cmd = new SqlCommand("[sp_get_all_active_orders]");

            cmd.Connection = conn;

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var order = new Order();
                        order.OrderID = reader.GetInt32(0);
                        var standardItem = new StandardItem();
                        standardItem.StandardItemID = reader.GetInt32(1);
                        var addOn = new AddOn();
                        addOn.Name = reader.GetString(2);
                        addOn.Description = reader.GetString(3);
                        standardItem.AddOns.Add(addOn);
                        order.StandardItem.Add(standardItem);
                        orders.Add(order);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return orders;
        }

        public List<Order> RetrieveAllOrdersByStatus(string status)
        {
            List<Order> orders = new List<Order>();

            var conn = DBConnection.GetConnection();

            var cmd = new SqlCommand("sp_get_order_by_status");

            cmd.Connection = conn;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50);
            cmd.Parameters["@Status"].Value = status;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var order = new Order();
                        order.OrderID = reader.GetInt32(0);
                        var standardItem = new StandardItem();
                        standardItem.StandardItemID = reader.GetInt32(1);
                        var addOn = new AddOn();
                        addOn.Name = reader.GetString(2);
                        addOn.Description = reader.GetString(3);
                        standardItem.AddOns.Add(addOn);
                        order.StandardItem.Add(standardItem);
                        orders.Add(order);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            
            return orders;
        }

        public bool UpdateOrderStatus(string status, int orderID)
        {
            bool result = false;

            // connection
            var conn = DBConnection.GetConnection();

            // command
            var cmd = new SqlCommand("sp_update_order_status");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@OrderID", SqlDbType.Int);
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50);


            // values
            cmd.Parameters["@OrderID"].Value = orderID;
            cmd.Parameters["@Status"].Value = status;

            // execute
            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                result = (rowsAffected == 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

       
    }
}


//Code to try and parse the data coming in as an Order
//Struggled because of the reapeating orderId and standardItemID
//Was not able to get it to properly read the Order from the database on the program side. 
//The way that I did it seem to work for the use cases I need now
//Plan on fixing next semester

//if (order.OrderID != reader.GetInt32(0))//OrderID will be null if it not set to anything
//                        {
//                            order.OrderID = reader.GetInt32(0);
//                            order.OrderID = reader.GetInt32(0);//Sets the order ID to the Reader(0) This value should not change until order.orderID Does not equal reader(0)
//                            StandardItem standardItem = new StandardItem();
//int tempStandardItem = reader.GetInt32(1);
//                            while (tempStandardItem != reader.GetInt32(1))
//                            {
//                                standardItem.StandardItemID = reader.GetInt32(1);
//                                AddOn addOns = new AddOn();
//                                while (addOns.Name != reader.GetString(2))
//                                {
//                                    addOns.Name = reader.GetString(2);
//                                    addOns.Description = reader.GetString(3);
//                                    reader.Read();
//                                    break;
//                                }
//                                standardItem.AddOns.Add(addOns);
//                            }
//                            standardItem.StandardItemID = reader.GetInt32(1);
//                            order.StandardItem.Add(standardItem);
//                            reader.Read();
//                        }
//                        else
//                        {
//                            order.OrderID = reader.GetInt32(0);
//                            reader.Read();
//                        }



//Another way I tried it
//var order = new Order();
//order.OrderID = reader.GetInt32(0);
//int addOnAmount = 0;
//int standardItemAmount = 0;
//if (reader.GetInt32(0) == order.OrderID)
//{
//    List<StandardItem> standardItemList = new List<StandardItem>();
//    var standardItem = new StandardItem();
//    standardItem.StandardItemID = reader.GetInt32(1);

//    standardItemList.Add(standardItem);
//    if (standardItem.StandardItemID == reader.GetInt32(1))
//    {
//        List<AddOn> addOns = new List<AddOn>();
//        var addOn = new AddOn();
//        addOn.Name = reader.GetString(2);
//        //addOns.Add(addOn);
//        standardItemList[standardItemAmount].AddOns.Add(addOn);
//        if (standardItemList[standardItemAmount].AddOns[addOnAmount].Name == reader.GetString(2))
//        {
//            break;
//        }
//        //addOnAmount++;
//        //order.StandardItem[standardItemAmount].AddOns[addOnAmount].Name = reader.GetString(2);
//        //standardItemList[standardItemAmount].AddOns = addOns;
//    }
//    standardItemAmount++;
//    order.StandardItem[standardItemAmount].StandardItemID = reader.GetInt32(1);

//}
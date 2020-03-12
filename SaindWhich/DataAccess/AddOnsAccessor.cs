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
    public class AddOnsAccessor : IAddOnsAccessor
    {
        public bool InsertAddOn(int orderID, int standardItemID, int ingredientID)
        {
            bool result = false;

            // connection
            var conn = DBConnection.GetConnection();

            // command
            var cmd = new SqlCommand("sp_insert_addon");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@OrderID", SqlDbType.Int);
            cmd.Parameters.Add("@StandardItemID", SqlDbType.Int);
            cmd.Parameters.Add("@IngredientID", SqlDbType.Int);

            // values
            cmd.Parameters["@OrderID"].Value = orderID;
            cmd.Parameters["@StandardItemID"].Value = standardItemID;
            cmd.Parameters["@IngredientID"].Value = ingredientID;

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

        public List<AddOn> RetrieveAllAddOns()
        {
            List<AddOn> addOns = new List<AddOn>();

            var conn = DBConnection.GetConnection();

            var cmd = new SqlCommand("sp_select_all_addons");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
                        
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AddOn addOn = new AddOn();
                    addOn.Name = reader.GetString(0);
                    addOn.IngredientID = reader.GetInt32(1);
                    addOns.Add(addOn);
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
            return addOns;
        }
    }
}

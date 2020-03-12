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
    public class StandardItemAccessor : IStandardItemAccessor
    {
        public int CreateBaseStandardItem()
        {
            int standardItemID = 0;
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_insert_standarditem", conn);
            cmd.CommandType = CommandType.StoredProcedure;
                       
            try
            {
                conn.Open();
                standardItemID = Convert.ToInt32(cmd.ExecuteScalar());
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return standardItemID;
        }
    }
}

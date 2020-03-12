using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataAccess
{
    internal static class DBConnection
    {
        private static string connectionString =
            //@"Data Source=DESKTOP-HVPRJEE\SQLEXPRESS;Initial Catalog=SaindwhichDB;Integrated Security=True";
        
            @"Data Source=localhost;Initial Catalog=SaindwhichDB;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}

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
    public class UserAccessor :  IUserAccessor
    {
        public User AuthenticateUser(string username, string passwordHash)
        {
            User user = null;
            int result = 0; // this will be 1 if the user is authenticated

            // start with a connection
            var conn = DBConnection.GetConnection();

            // next, get a command object
            var cmd = new SqlCommand("sp_authenticate_user");
            cmd.Connection = conn;

            // set the command type
            cmd.CommandType = CommandType.StoredProcedure;

            // add the parameters to the command
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            // provide values for the parameters
            cmd.Parameters["@Email"].Value = username;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            // now that the command is ready,
            // open a connection, execute and process results
            try
            {
                conn.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());

                if (result == 1)
                {
                    user = SelectUserByEmail(username);
                }
                else
                {
                    throw new ApplicationException("User not found!");
                }
            }
            catch (Exception up)
            {
                throw up;
            }
            return user;
        }

        public int insertEmployee(User user)
        {
            int employeeID = 0;

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_insert_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            cmd.Parameters.AddWithValue("@Email", user.Email);



            try
            {
                conn.Open();
                employeeID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return employeeID;
        }

        public User SelectUserByID(int id)
        {
            User user = new User();

            var conn = DBConnection.GetConnection();

            var cmd = new SqlCommand("sp_select_employee_by_id");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters["@EmployeeID"].Value = id;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user.EmployeeID = reader.GetInt32(0);
                        user.FirstName = reader.GetString(1);
                        user.LastName = reader.GetString(2);
                        user.PhoneNumber = reader.GetString(3);
                        user.Email = reader.GetString(4);
                        
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
            return user;
        }

        public List<User> SelectUsersByActive(bool active = true)
        {
            List<User> users = new List<User>();

            var conn = DBConnection.GetConnection();

            var cmd = new SqlCommand("sp_select_users_by_active");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Active", SqlDbType.Bit);
            cmd.Parameters["@Active"].Value = active;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var user = new User();
                        user.EmployeeID = reader.GetInt32(0);
                        user.FirstName = reader.GetString(1);
                        user.LastName = reader.GetString(2);
                        user.PhoneNumber = reader.GetString(3);
                        user.Email = reader.GetString(4);
                        user.Active = active;

                        users.Add(user);
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
            return users;
        }

        public int updateEmployee(User oldUser, User newUser)
        {
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_update_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmployeeID", oldUser.EmployeeID);

            cmd.Parameters.AddWithValue("@NewFirstName", newUser.FirstName);
            cmd.Parameters.AddWithValue("@NewLastName", newUser.LastName);
            cmd.Parameters.AddWithValue("@NewPhoneNumber", newUser.PhoneNumber);
            cmd.Parameters.AddWithValue("@NewEmail", newUser.Email);

            cmd.Parameters.AddWithValue("@OldFirstName", oldUser.FirstName);
            cmd.Parameters.AddWithValue("@OldLastName", oldUser.LastName);
            cmd.Parameters.AddWithValue("@OldPhoneNumber", oldUser.PhoneNumber);
            cmd.Parameters.AddWithValue("@OldEmail", oldUser.Email);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ApplicationException("Record not found");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public bool UpdatePasswordHash(int userID, string oldPassHash, string newPassHash)
        {
            bool result = false;

            // connection
            var conn = DBConnection.GetConnection();

            // command
            var cmd = new SqlCommand("sp_update_password");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);

            // values
            cmd.Parameters["@EmployeeID"].Value = userID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPassHash;
            cmd.Parameters["@NewPasswordHash"].Value = newPassHash;

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

        public User SelectUserByEmail(string email)
        {
            User user = null;

            // connection?
            var conn = DBConnection.GetConnection();

            // commands?
            var cmd1 = new SqlCommand("sp_select_user_by_email", conn);
            var cmd2 = new SqlCommand("sp_select_roles_by_userid", conn);

            // command type?
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd2.CommandType = CommandType.StoredProcedure;

            // parameters?
            cmd1.Parameters.Add("@Email", SqlDbType.NVarChar, 250);
            cmd2.Parameters.Add("@EmployeeID", SqlDbType.Int);

            // values (need to wait for cmd2's parameter)
            cmd1.Parameters["@Email"].Value = email;

            try
            {
                // open the connection
                conn.Open();

                var reader1 = cmd1.ExecuteReader();

                user = new User();

                user.Email = email;
                if (reader1.Read())
                {
                    user.EmployeeID = reader1.GetInt32(0);
                    user.FirstName = reader1.GetString(1);
                    user.LastName = reader1.GetString(2);
                    user.PhoneNumber = reader1.GetString(3);
                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
                reader1.Close();
                // now cmd2 needs a parameter value
                cmd2.Parameters["@EmployeeID"].Value = user.EmployeeID;

                var reader2 = cmd2.ExecuteReader();

                List<string> roles = new List<string>();
                while (reader2.Read())
                {
                    roles.Add(reader2.GetString(0));
                }
                reader2.Close();

                user.Roles = roles;
            }
            catch (Exception up)
            {
                throw up;
            }
            finally
            {
                conn.Close();
            }
            return user;
        }
        public List<string> SelectAllRoles()
        {
            List<string> roles = new List<string>();

            // connection
            var conn = DBConnection.GetConnection();

            // command objects
            var cmd = new SqlCommand("sp_select_all_roles");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                // open connection
                conn.Open();

                // execute the first command

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string role = reader.GetString(0);
                    roles.Add(role);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roles;
        }

        public List<string> SelectRolesByEmployeeID(int employeeID)
        {
            List<string> roles = new List<string>();

            // connection
            var conn = DBConnection.GetConnection();

            // command objects
            var cmd = new SqlCommand("sp_select_roles_by_userid");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters["@EmployeeID"].Value = employeeID;

            try
            {
                // open connection
                conn.Open();

                // execute the first command

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string role = reader.GetString(0);
                    roles.Add(role);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roles;
        }

        public int InsertOrDeleteEmployeeRole(int employeeID, string role, bool delete = false)
        {
            int rows = 0;

            string cmdText = delete ? "sp_delete_employee_role" : "sp_insert_employee_role";

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
            cmd.Parameters.AddWithValue("@RoleID", role);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

    }
}

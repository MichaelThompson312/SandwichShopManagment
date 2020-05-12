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
    public class UserManager : IUserManager
    {
        private IUserAccessor _userAccessor;

        public UserManager(IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        public UserManager()
        {
            _userAccessor = new UserAccessor();
        }

        public bool AddEmployee(User user)
        {
            bool result = false;

            try
            {
                result = (_userAccessor.insertEmployee(user) > 0);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Add failed.", ex);
            }
            return result;
        }

        public User AuthenticateUser(string email, string password)
        {
            User user = null;
            try
            {
                var passwordHash = hashPassword(password);
                password = null;

                user = _userAccessor.AuthenticateUser(email, passwordHash);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bad email address or password.", ex);
            }
            return user;
        }

        public bool EditEmployee(User oldUser, User newUser)
        {
            bool result = false;

            try
            {
                result = (1 == _userAccessor.updateEmployee(oldUser, newUser));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }

        public User GetUserByID(int id)
        {
            try
            {
                return _userAccessor.SelectUserByID(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("User not found", ex);
                throw;
            }
        }

        public List<User> GetUserListByActive(bool active = true)
        {
            try
            {
                return _userAccessor.SelectUsersByActive(active);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("List Not Available", ex);
            }
        }

        public bool ResetPassword(int employeeID, string oldPassword, string newPassword)
        {
            bool result = false;

            try
            {
                string oldHash = hashPassword(oldPassword);
                string newHash = hashPassword(newPassword);

                result = _userAccessor.UpdatePasswordHash(employeeID,
                    oldHash, newHash);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed.", ex);
            }
            return result;
        }

        // password hash method
        private string hashPassword(string source)
        {
            string result = "";

            // create a byte array - cryptography uses bits and bytes
            byte[] data;

            using (SHA256 sha256hash = SHA256.Create())
            {
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
                var s = new StringBuilder();
                // loop through the bytes and build the string
                for (int i = 0; i < data.Length; i++)
                {
                    s.Append(data[i].ToString("x2"));
                }
                result = s.ToString().ToUpper();
            }
            return result;
        }

        public List<string> RetrieveEmployeeRoles(int employeeID)
        {
            List<string> roles = null;

            try
            {
                roles = _userAccessor.SelectRolesByEmployeeID(employeeID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }

            return roles;
        }

        public List<string> RetrieveEmployeeRoles()
        {
            List<string> roles = null;

            try
            {
                roles = _userAccessor.SelectAllRoles();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }

            return roles;
        }

        public bool FindUser(string email)
        {
            try
            {
                return _userAccessor.SelectUserByEmail(email) != null;
            }
            catch (ApplicationException ax)
            {
                if(ax.Message == "User not found.")
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database error", ex);
            }
        }

        public int RetrieveUserIDFromEmail(string email)
        {
            try
            {
                return _userAccessor.SelectUserByEmail(email).EmployeeID;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database Error", ex);
            }
        }

        public bool AddUserRole(int employeeID, string role)
        {
            bool result = false;
            try
            {
                result = (1 == _userAccessor.InsertOrDeleteEmployeeRole(employeeID, role));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Role not added!", ex);
            }
            return result;
        }
        public bool DeleteUserRole(int employeeID, string role)
        {
            bool result = false;
            try
            {
                result = (1 == _userAccessor.InsertOrDeleteEmployeeRole(employeeID, role, delete: true));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Role not removed!", ex);
            }
            return result;
        }
    }
}

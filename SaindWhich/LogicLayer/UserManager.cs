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

        public UserManager()
        {
            _userAccessor = new UserAccessor();
        }

        public UserManager(IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
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


    }
}

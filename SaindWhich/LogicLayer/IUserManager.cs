using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayer
{
    public interface IUserManager
    {
        User AuthenticateUser(string email, string password);

        bool ResetPassword(int employeeID, string oldPassword, string newPassword);

        List<User> GetUserListByActive(bool active = true);

        bool EditEmployee(User oldUser, User newUser);
        bool AddEmployee(User user);



    }
}

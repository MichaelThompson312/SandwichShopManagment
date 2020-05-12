using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccess
{
    public interface IUserAccessor
    {
        User AuthenticateUser(string username, string passwordHash);

        bool UpdatePasswordHash(int userID,
            string oldPassHash, string newPassHash);

        List<User> SelectUsersByActive(bool active = true);

        int updateEmployee(User oldUser, User newUser);

        int insertEmployee(User user);
        User SelectUserByID(int id);

        List<string> SelectAllRoles();

        List<string> SelectRolesByEmployeeID(int employeeID);

        User SelectUserByEmail(string email);

        int InsertOrDeleteEmployeeRole(int employeeID, string role, bool delete = false);

    }
}

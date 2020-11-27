using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UsersTest.Models.Entities;

namespace UsersTest.Models.Interfaces
{
    public interface IDataProvider
    {
        public IEnumerable<User> GetAllUsers();
        public IEnumerable<Role> GetAllRoles();
        public int AddUser(User newUser);
        public bool EditUser(User editedUser);
        public bool DeleteUser(User deletedUser);
    }
}

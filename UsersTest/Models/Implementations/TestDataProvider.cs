using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UsersTest.Models.Entities;
using UsersTest.Models.Interfaces;

namespace UsersTest.Models.Implementations
{
    public class TestDataProvider : IDataProvider
    {
        private List<User> AllUsers = new List<User>();
        private List<Role> AllRoles = new List<Role>();

        public TestDataProvider()
        {
            AllRoles.Add(new Role
            {
                Id = 0,
                Name = "Admin"
            });
            AllRoles.Add(new Role
            {
                Id = 1,
                Name = "User"
            });
            AllUsers.Add(new User
            {
                Id = 0,
                Email = "test@test.com",
                Login = "testUser",
                Name = "Peter",
                Password = "qwerty",
                Roles = AllRoles.ToList()
            });
            AllUsers.Add(new User
            {
                Id = 1,
                Email = "test2@test.com",
                Login = "test2User",
                Name = "Jack",
                Password = "qwerty2#",
                Roles = AllRoles.Where(i => i.Id == 1).ToList()
            });
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return AllRoles;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return AllUsers;
        }

        public int AddUser(User newUser)
        {
            newUser.Id = AllUsers.Last().Id + 1;
            AllUsers.Add(newUser);
            return newUser.Id;
        }

        public bool EditUser(User editedUser)
        {
            User usr = AllUsers.FirstOrDefault(usr => usr.Id == editedUser.Id);
            if(usr == null)
            {
                return false;
            }
            usr.Name = editedUser.Name;
            usr.Password = editedUser.Password;
            usr.Roles = editedUser.Roles;
            usr.Email = editedUser.Email;
            return true;
        }

        public bool DeleteUser(User deletedUser)
        {
            User tmp = AllUsers.FirstOrDefault(i => i.Id == deletedUser.Id);
            if(tmp == null)
            {
                return false;
            }
            AllUsers.Remove(tmp);
            return true;
        }
    }
}

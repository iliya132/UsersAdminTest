using System.Collections.Generic;
using UsersTest.Models.Entities.Base;

namespace UsersTest.Models.Entities
{
    public class User :NamedEntity
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}

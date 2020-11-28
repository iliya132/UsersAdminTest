using Microsoft.AspNetCore.Mvc;

using UsersTest.Models.Implementations;
using UsersTest.Models.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using UsersTest.Models.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace UsersTest.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IDataProvider _dbProvider;
        public UsersController(IDataProvider provider)
        {
            _dbProvider = provider;
        }

        [HttpGet]
        [Authorize]
        [Route("allUsers")]
        public IActionResult GetUsers()
        {
            
            return new JsonResult(_dbProvider.GetAllUsers());
        }

        [HttpGet]
        [Route("roles")]
        public IActionResult GetRoles()
        {
            return new JsonResult(_dbProvider.GetAllRoles());
        }

        [HttpPost]
        [Route("adduser")]
        public int AddUser(User newUser)
        {
            return _dbProvider.AddUser(newUser);
        }

        [HttpPut]
        [Route("edituser")]
        public bool EditUser(User newUser)
        {
           
            return _dbProvider.EditUser(newUser);
        }

        [HttpDelete]
        [Route("deleteuser")]
        public bool DeleteUser(User newUser)
        {
            return _dbProvider.DeleteUser(newUser);
        }
    }
}

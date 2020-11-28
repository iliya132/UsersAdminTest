using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using UsersTest.Models.Entities;
using UsersTest.Models.Interfaces;

namespace UsersTest.Controllers
{
    [Route("Users")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IDataProvider _dbProvider;
        public UsersController(IDataProvider provider)
        {
            _dbProvider = provider;
        }

        [HttpGet]
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
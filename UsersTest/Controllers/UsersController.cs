using Microsoft.AspNetCore.Mvc;

using UsersTest.Models.Implementations;
using UsersTest.Models.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using UsersTest.Models.Entities;
using System.Threading.Tasks;

namespace UsersTest.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IDataProvider _dbProvider;
        public UsersController()
        {
            _dbProvider = new TestDataProvider();
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
    }
}

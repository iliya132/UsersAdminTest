using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using UsersTest.Auth;
using UsersTest.Models.Entities.ViewModels;

namespace UsersTest.Controllers
{
    [Route("jwt/")]
    [ApiController]
    public class AccController : ControllerBase
    {


        [HttpPost("token")]
        public IActionResult Token(AccountViewModel vm)
        {
            //работа JWT исключительно демонстрационная. Пользователи приложений хранятся в БД, пароли лежат как хэши
            // Здесь бы я обратился к методу в БД, проверяющему корректность пользовательских данных
            if(vm.UserName == null || vm.Password == null || 
                !(vm.UserName.Equals("TestUser") && vm.Password.Equals("testPasswrd")))
            {
                return BadRequest();
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, "TestUser"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin")
            };
            DateTime now = DateTime.UtcNow;
            JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            TokenVM tk = new TokenVM
            {
                UserName = vm.UserName,
                Token = encodedJwt
            };
            return new JsonResult(tk);
        }
    }
}

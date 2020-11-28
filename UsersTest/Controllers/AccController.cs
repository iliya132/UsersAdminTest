using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using UsersTest.Auth;

namespace UsersTest.Controllers
{
    [Route("jwt/")]
    [ApiController]
    public class AccController : ControllerBase
    {
        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            if(!(username.Equals("TestUser") && password.Equals("testPasswrd")))
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

            (string accessToken, string userName) response = (
                accessToken: encodedJwt,
                userName: username
            );

            return new JsonResult(response);
        }
    }
}

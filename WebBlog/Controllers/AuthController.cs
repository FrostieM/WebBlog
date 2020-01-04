using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebBlog.Helpers;
using WebBlog.Model;
using WebBlog.Model.Interfaces.Repositories;
using WebBlog.Model.Forms;

namespace WebBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public AuthController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost, Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Post([FromBody]LoginForm userForm)
        {
            var user = _repository.Users.FirstOrDefault(u => 
                u.UserName == userForm.UserName &&
                u.Password == userForm.Password);
            
            if (user == null) 
                return NotFound("User not found");

            return Ok(new { token = GenerateToken(user.UserName) });
        }

        [HttpPost, Route("signUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] SignUpForm userForm)
        {
            var userExist = _repository.Users.Any(u => u.UserName == userForm.UserName);

            if (userExist) return BadRequest("User already exist");
            
            _repository.SaveUser(new User
            {
                UserName = userForm.UserName,
                Password = userForm.Password,
                Email = userForm.Email,
                FirstName = userForm.FirstName,
                LastName = userForm.LastName
            });
            
            return Ok(new { token = GenerateToken(userForm.UserName) });
        }
        
        private static string GenerateToken(string username)
        {
            var identity = GetIdentity(username);
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                AuthOptions.Issuer,
                AuthOptions.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), 
                    SecurityAlgorithms.HmacSha256));
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        
        private static ClaimsIdentity GetIdentity(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
            };
                
            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
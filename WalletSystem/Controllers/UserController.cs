using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using BusinessLayer;

namespace WalletSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUsers _user;
        public UserController(IConfiguration configuration, IUsers user) 
        {
            _configuration = configuration; 
            _user = user;
        }

        private string GenerateJwtToken(UsersModelObject user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsersModelObject usersObject)
        {

            try
            {
                if(string.IsNullOrWhiteSpace(usersObject.Username) || string.IsNullOrWhiteSpace(usersObject.Password))
                {
                    return BadRequest(new { Message = "Username and password is required." });
                }

                if (await _user.CheckUserExist(usersObject))
                {
                    return BadRequest(new { Message = "Username already exists." });
                }

                if(!await _user.Register(usersObject))
                {
                    return BadRequest(new { Message = "Invalid registration details. Please try again." });
                }

                return Ok(new
                {
                    Message = "User registration successful!"
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsersModelObject usersObject)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usersObject.Username) || string.IsNullOrWhiteSpace(usersObject.Password))
                {
                    return BadRequest(new { Message = "Username or password." });
                }

                if (!await _user.Verify(usersObject))
                {
                    return BadRequest(new { Message = "Username or password." });
                }

                var token = GenerateJwtToken(usersObject);

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { Message = "Invalid token." });
                }

                Response.Headers.Add("Authorization", "Bearer " + token);

                return Ok(new
                {
                    Message = "User login sucessful!",
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

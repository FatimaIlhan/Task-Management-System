using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Task_Management_API.DTOs;
using Task_Management_API.Models;

namespace Task_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TaskDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public UserController(TaskDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var objUser = _dbContext.Users.FirstOrDefault(x => x.Email == userDTO.Email);
            if (objUser == null)
            {
                _dbContext.Users.Add(new User
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    Password = userDTO.Password


                });
                _dbContext.SaveChanges();
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest("User already exist with the same email address");
            }
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password);
            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("UserId",user.Id.ToString()),
                    new Claim("Email", user.Email.ToString())


                };
                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(

                    _configuration["Jwt:Issuer"],
                     _configuration["Jwt:Audience"],
                     claims,
                     expires: DateTime.UtcNow.AddMinutes(60),
                     signingCredentials: signIn

                    );

                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
               // return Ok(user);
               return Ok(new {Token = tokenValue,Use = user
               });
            }
            return NoContent();
        }
        [Authorize]
        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
            if(user != null)
            {
                return Ok(user);
            }
            return NoContent();
        }
    }
}
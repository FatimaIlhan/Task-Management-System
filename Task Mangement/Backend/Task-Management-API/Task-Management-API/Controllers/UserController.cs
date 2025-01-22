using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_Management_API.DTOs;
using Task_Management_API.Models;

namespace Task_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TaskDbContext _dbContext;
        public UserController(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
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
                return Ok(user);
            }
            return NoContent();
        }
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
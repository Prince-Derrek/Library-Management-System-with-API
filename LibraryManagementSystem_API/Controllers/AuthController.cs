using Microsoft.AspNetCore.Mvc;    //enables API behaviour
using LibraryManagementSystem_API.Data;  //provides access to my library context
using LibraryManagementSystem_API.DTOs;  //allows me to use my register and login DTOs
using LibraryManagementSystem_API.Models; //lets me work with my user model
using Microsoft.AspNetCore.Identity; //enables password hashing
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; //for querying the database

namespace LibraryManagementSystem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public readonly LibraryContext _context;
        public AuthController(LibraryContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterDTO registerDTO)
        {
            if (await _context.Users.AnyAsync(u => u.userName == registerDTO.userName))
            {
                return BadRequest("Username is aready taken!");//400
            }

            var user = new User
            {
                userName = registerDTO.userName
            };
            
            var hasher = new PasswordHasher<User>();
            user.passwordHash = hasher.HashPassword(user, registerDTO.userPassword);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User Registered Successfully");
        }
    }
}

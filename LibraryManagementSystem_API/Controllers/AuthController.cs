using Microsoft.AspNetCore.Mvc;    //enables API behaviour
using LibraryManagementSystem_API.Data;  //provides access to my library context
using LibraryManagementSystem_API.DTOs;  //allows me to use my register and login DTOs
using LibraryManagementSystem_API.Models; //lets me work with my user model
using Microsoft.AspNetCore.Identity; //enables password hashing
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; //for querying the database
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;  //for TokenValidationParameters, SymmetricSecurityKey
using System.IdentityModel.Tokens.Jwt;  //for JwtSecurityTokenHandler
using Microsoft.AspNetCore.Identity; //for password hasher
using System.Text;   //for encoding.UTF8
using System.Security.Claims;  //for Claim[]

namespace LibraryManagementSystem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public readonly LibraryContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(LibraryContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            Console.WriteLine("JWT Key: " + _configuration["Jwt:Key"]);

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

        [HttpPost("login")]
        public async Task<ActionResult<string>> UserLogin([FromBody] LoginDTO loginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.userName == loginDTO.userName);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.passwordHash, loginDTO.userPassword);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid password");
            }

            //Creating a JWT Token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.userName),
                new Claim(ClaimTypes.NameIdentifier, user.userID.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString });
        }
    }
}

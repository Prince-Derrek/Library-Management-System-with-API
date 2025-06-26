using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LibraryManagementSystem_API.Data;
using LibraryManagementSystem_API.Models;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public readonly LibraryContext _context;
        public UserController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserByID(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddNewUser(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserByID), new { id = newUser.userID }, newUser);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUserFullDetails(int id, User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                return NotFound();
            }

            user.userName = updatedUser.userName;
            user.passwordHash = updatedUser.passwordHash;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUserByID(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

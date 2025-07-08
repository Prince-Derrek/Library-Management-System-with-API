using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LibraryManagementSystem_API.Data;
using LibraryManagementSystem_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using LibraryManagementSystem_API.DTOs;

namespace LibraryManagementSystem_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        public readonly LibraryContext _context;
        public BookController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Book>> GetAllBooks()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(books);//200
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookByID(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if(book == null)
            {
                return NotFound();//404
            }

            return Ok(book);//200
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Book>> AddNewBook(Book newBook)
        {
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookByID), new {id = newBook.bookID}, newBook);//201
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> UpdateBookFullDetails(int id, Book updatedBook)
        {
            var book = await _context.Books.FindAsync(id);

            if(book == null)
            {
                return NotFound();//404
            }

            book.bookAuthor = updatedBook.bookAuthor;
            book.bookTitle = updatedBook.bookTitle;

            await _context.SaveChangesAsync();//save changes to the DB


            return NoContent();//204
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBookByID(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if(book == null)
            {
                return NotFound();//404
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync(); // Save Changes


            return NoContent();
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("{id}/borrow")]
        public async Task<ActionResult<Book>> BorrowBookByID(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if(book == null)
            {
                return NotFound("Book not Found");
            }

            var username = User.Identity?.Name;

            book.isBorrowed = true;
            book.borrowedBy = username;
            book.borrowedAt = DateTime.UtcNow;
            book.returnedAt = null;

            await _context.SaveChangesAsync();

            return Ok("Book Borrowed Successfully!");
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookDTO dto)
        {
            var book = await _context.Books.FindAsync(dto.bookId);
            if (book == null)
            {
                return NotFound("No Such book exists");
            }
            if (!book.isBorrowed)
                return BadRequest("Book is already marked as returned");
            book.isBorrowed = false;
            book.borrowedBy = null;
            book.borrowedAt = null;
            book.returnedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok("Book Returned Successfully");
        }
    }
}

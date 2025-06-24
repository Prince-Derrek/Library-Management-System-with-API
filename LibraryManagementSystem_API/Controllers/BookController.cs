using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LibraryManagementSystem_API.Data;
using LibraryManagementSystem_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem_API.Controllers
{
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

        [HttpPost]
        public async Task<ActionResult<Book>> AddNewBook(Book newBook)
        {
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookByID), new {id = newBook.bookID}, newBook);//201
        }

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

    }
}

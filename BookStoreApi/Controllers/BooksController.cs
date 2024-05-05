using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService)
        {
            _booksService = booksService;
        }

        // Endpoint untuk mendapatkan semua buku
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _booksService.GetAsync();
            return Ok(books);
        }

        // Implementasikan endpoint lainnya (POST, PUT, DELETE) sesuai kebutuhan

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> GetBookById(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(Book newBook)
        {
            await _booksService.CreateAsync(newBook);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateBook(string id, Book updatedBook)
        {
            var book = await _booksService.GetAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            updatedBook.Id = id;
            await _booksService.UpdateAsync(id, updatedBook);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var book = await _booksService.GetAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _booksService.RemoveAsync(id);
            return NoContent();
        }
    }
}

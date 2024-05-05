using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorsService _authorsService;

        public AuthorsController(AuthorsService authorsService)
        {
            _authorsService = authorsService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Author>))]
        public async Task<ActionResult<IEnumerable<Author>>> GetAllAuthors()
        {
            var authors = await _authorsService.GetAllAsync();
            return Ok(authors);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(200, Type = typeof(Author))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Author>> GetAuthorById(string id)
        {
            var author = await _authorsService.GetAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Author))]
        public async Task<IActionResult> CreateAuthor(Author newAuthor)
        {
            await _authorsService.CreateAsync(newAuthor);
            return CreatedAtAction(nameof(GetAuthorById), new { id = newAuthor.Id }, newAuthor);
        }

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateAuthor(string id, Author updatedAuthor)
        {
            var existingAuthor = await _authorsService.GetAsync(id);

            if (existingAuthor == null)
            {
                return NotFound();
            }

            updatedAuthor.Id = existingAuthor.Id;

            await _authorsService.UpdateAsync(id, updatedAuthor);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAuthor(string id)
        {
            var existingAuthor = await _authorsService.GetAsync(id);

            if (existingAuthor == null)
            {
                return NotFound();
            }

            await _authorsService.RemoveAsync(id);

            return NoContent();
        }
    }
}

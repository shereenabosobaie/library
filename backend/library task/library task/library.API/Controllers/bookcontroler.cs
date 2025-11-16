using libaraey.APPLICATION.library.Application.dto;
using library_mangment.library.Application.dto;
using library_mangment.library.Application.@interface;
using library_mangment.library.domain.entities;
using library_task.library.Application.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace library_mangment.library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class bookcontroler : Controller
    {
        private readonly IBookServies bookServies;
        private readonly IWebHostEnvironment _env;

        public bookcontroler(IBookServies bookServies, IWebHostEnvironment env)
        {
            this.bookServies = bookServies;
            _env = env;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<book>))]
        public IActionResult Getbooks()
        {
            var books = bookServies.GetAllBooks();
            return Ok(books);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBook([FromForm] CreateBookDto bookDto, IFormFile? image)
        {
            if (bookDto == null)
                return BadRequest(ModelState);

            string? imageUrl = null;

            if (image != null)
            {
                var uploadDir = Path.Combine(_env.WebRootPath, "images");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var uniqueName = $"{Guid.NewGuid()}_{image.FileName}";
                var filePath = Path.Combine(uploadDir, uniqueName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imageUrl = $"/images/{uniqueName}";
            }

            bookDto.ImageUrl = imageUrl;

            var result = bookServies.CreateBook(bookDto);
            if (!result)
            {
                ModelState.AddModelError("", "Something went wrong while saving or book already exists");
                return StatusCode(500, ModelState);
            }

            return Ok(new { message = "Successfully created", imageUrl });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("update/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateBook(int bookId, [FromForm] BookDto updatedBook, IFormFile? image)
        {
            if (updatedBook == null)
                return BadRequest(ModelState);

            if (image != null)
            {
                var uploadDir = Path.Combine(_env.WebRootPath, "images");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var uniqueName = $"{Guid.NewGuid()}_{image.FileName}";
                var filePath = Path.Combine(uploadDir, uniqueName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                updatedBook.ImageUrl = $"/images/{uniqueName}";
            }

            var result = bookServies.UpdateBook(bookId, updatedBook);
            if (!result)
            {
                ModelState.AddModelError("", "Something went wrong updating book");
                return StatusCode(500, ModelState);
            }

            return Ok(new { message = "Successfully Updated" });
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBook(int bookId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = bookServies.DeleteBook(bookId);
            if (!result)
            {
                ModelState.AddModelError("", "Something went wrong deleting book");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpGet("search")]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest();

            var results = bookServies.GetAllBooks().Where(b => b.Title.ToLower().Contains(query.ToLower()) || b.Author.ToLower().Contains(query.ToLower())).ToList();

            return Ok(results);
        }
        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = bookServies.GetBookById(id);
            if (book == null)
                return NotFound("Book not found.");

            return Ok(book);
        }

        [HttpGet("recent")]
        public IActionResult GetRecent()
        {
            var books = bookServies.recent();
            return Ok(books);
        }
    }
}
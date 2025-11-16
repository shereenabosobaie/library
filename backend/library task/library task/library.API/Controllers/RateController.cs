using libaraey.APPLICATION.library.Application;
using libaraey.APPLICATION.library.Application.dto;
using Microsoft.AspNetCore.Mvc;

namespace library_task.library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RateController : ControllerBase
    {
        private readonly IrateService _service;

        public RateController(IrateService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult AddRate([FromBody] ratedto dto)
        {
            if (_service.AddRate(dto))
                return Ok(new { message = "Rate added successfully" });

            return BadRequest(new { message = "Failed to add rate" });
        }

        [HttpGet("average/{bookId}")]
        public IActionResult GetAverageRating(int bookId)
        {
            var average = _service.GetAverageRating(bookId);
            return Ok(new { bookId, average });
        }

        [HttpGet("book/{bookId}")]
        public IActionResult GetBookRates(int bookId)
        {
            var rates = _service.GetBookRates(bookId);
            return Ok(rates);
        }

        [HttpGet("{memberId}/{bookId}")]
        public IActionResult GetRate(int memberId, int bookId)
        {
            var rate = _service.GetRate(memberId, bookId);
            if (rate == null) return NotFound();
            return Ok(rate);
        }
    }
}

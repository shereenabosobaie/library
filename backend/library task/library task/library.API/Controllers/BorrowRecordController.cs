using library_mangment.library.Application.dto;
using library_mangment.library.Application.@interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace library_mangment.library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BorrowRecordController : ControllerBase
    {
        
        private readonly IBorrowReturnService borrowServices;

        public BorrowRecordController(IBorrowReturnService borrowService)
        {
            borrowServices = borrowService;
        }
        [HttpPost("borrow")]
        public IActionResult Borrow([FromBody] BorrowRecordDto dto)
        {
            var memberIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (memberIdClaim == null) {
                
                return Unauthorized(); }
            
            var memberId = int.Parse(memberIdClaim);
            var success = borrowServices.BorrowBook(memberId, dto.bookId);
            if (!success) return BadRequest("Cannot borrow book");

            return Ok("Borrowed successfully");
        }

        [HttpPost("return")]
        
        public IActionResult Return([FromBody] BorrowRecordDto dto)
        {
            var memberIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (memberIdClaim == null) return Unauthorized();

            var memberId = int.Parse(memberIdClaim);
            var success = borrowServices.ReturnBook(memberId, dto.bookId);
            if (!success) return BadRequest("Cannot return book ");

            return Ok("Returned successfully");
        }
    }
}

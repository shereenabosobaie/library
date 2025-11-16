using libaraey.APPLICATION.library.Application;
using libaraey.APPLICATION.library.Application.dto;
using library.intrastructure.library.Infrastructure.rabbit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace library_task.library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : Controller
    {
        private readonly IrequestService _service;
        public RequestController(IrequestService service)
        {
            _service = service;
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Approve(int id, [FromQuery] string ended = "Approved")
        {
            var success = _service.ApproveRequest(id, ended);
            if (!success)
                return NotFound(new { message = "Request not found" });

            return Ok(new { message = $"Request {ended}" });
        }

        [HttpDelete("cancel/{id}")]
        [Authorize(Roles = "member")]
        public IActionResult Cancel(int id)
        {
            var memberIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (memberIdClaim == null) return Unauthorized();

            var memberId = int.Parse(memberIdClaim);
            var success = _service.CancelRequest(id, memberId);
            if (!success) return NotFound("Request not found or unauthorized");

            return Ok(new { message = "Request canceled successfully" });
        }
        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RejectRequest(int id)
        {
            var success = _service.ApproveRequest(id, "Rejected");
            if (!success) return NotFound(new { message = "Request not found." });
            return Ok(new { message = "Request rejected successfully." });
        }
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllRequests()
        {
            var requests = _service.GetAllRequests().ToList();
            return Ok(requests ?? new List<requestDTO>());
        }
        [HttpGet("member")]
        [Authorize(Roles = "member,Admin")]
        public IActionResult GetMemberRequests()
        {
            var memberIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"[DEBUG] Token MemberId: {memberIdClaim}");

            if (memberIdClaim == null)
                return Unauthorized("No member ID found in token.");

            int memberId = int.Parse(memberIdClaim);

            var requests = _service.GetMemberRequests(memberId).ToList();
            Console.WriteLine($"[DEBUG] Returning {requests.Count} requests for MemberId={memberId}");
            foreach (var r in requests)
                Console.WriteLine($"   -> ReqId={r.Id}, BookId={r.bookId}, MemberId={r.memberId}, Ended={r.ended}");

            return Ok(requests);
        }

        [HttpPost("create")]
        [Authorize(Roles = "member")]
        public async Task<IActionResult> CreateAsync([FromBody] requestDTO dto)
        {
            var memberIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            Console.WriteLine($" Token Claims -> MemberId: {memberIdClaim}, Email: {emailClaim}");

            if (memberIdClaim == null)
                return Unauthorized("No member ID found in token.");

            dto.memberId = int.Parse(memberIdClaim);

            var success = await _service.CreateRequest(dto);
            if (!success) return BadRequest("Invalid borrow request data");

            return Ok(new { message = "Borrow request created successfully" });
        }

        [HttpGet("test-notification")]
        public async Task<IActionResult> TestNotification([FromServices] IHubContext<NotificationHub> hubContext)
        {
            await hubContext.Clients.Group("Admins").SendAsync("NewBorrowRequest", new
            {
                Id = 999,
                bookId = 1,
                memberId = 42,
                days = 14,
                ended = "Pending"
            });

            Console.WriteLine("Test notification sent to Admins group!");

            return Ok("Notification sent successfully!");
        }
    }
}

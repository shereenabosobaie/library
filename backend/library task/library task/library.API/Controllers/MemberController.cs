using library_mangment.library.Application.dto;
using library_mangment.library.Application.@interface;
using library_mangment.library.domain.entities;
using library_task.library.Application.services;

//using library_mangment.library.Infrastructure.@interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace library_mangment.library.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : Controller
    {
        private readonly IMemberService MemberService;
        private readonly IauthService authService;
        public MemberController(IMemberService MemberService, IauthService authService)
        {
            this.MemberService = MemberService;
            this.authService = authService;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<member>))]
        public IActionResult Getmembers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var members = MemberService.GetAllMembers();
            return Ok(members);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] MemberDto request, IFormFile? file)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                if (file != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "members");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    request.ImageUrl = $"uploads/members/{fileName}";
                }
                var success = MemberService.Register(request);

                if (!success)
                    return Conflict(); 

                return Ok(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                return StatusCode(500); 
            }
        }

        [HttpPost("login")]
        public ActionResult<string> login(logindto request)
        {
            var mwmber = MemberService.Login(request);
            if (mwmber == string.Empty)
                return BadRequest("Invalid email or password");
            return Ok(new { Token = mwmber });
        }


        [HttpPut("update")]
        public IActionResult UpdateMember([FromBody] MemberDto dto)
        {
            var success = MemberService.UpdateMemberInfo(dto);
            if (!success)
                return NotFound("Member not found or update failed");

            return Ok("Member updated successfully");

        }
        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                return Unauthorized("No token provided");

            var userId = authService.ValidateToken(token);
            if (userId == null)
                return Unauthorized("Invalid token");

            var user = MemberService.GetById(userId.Value);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.ImageUrl,
                user.role
            });
        }
    }
    }
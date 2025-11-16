using libaraey.APPLICATION.library.Application.dto;
using library.DOMAIN.library.domain;
using library.DOMAIN.library.domain.entities;
using library.intrastructure.library.Infrastructure.rabbit;
using library_mangment.library.Application.@interface;
using library_mangment.library.domain.@interface;
using library_mangment.library.Infrastructure.@interface;
using library_mangment.library.Infrastructure.Repos;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libaraey.APPLICATION.library.Application.services
{
    public class requestService : IrequestService
    {
        private readonly IrequestRepo _repo;
        private readonly IBookRepo _bookRepo;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMemberRepo _MemberRepo;
        public requestService(IrequestRepo repo, IBookRepo bookRepo, IHubContext<NotificationHub> hubContext, IMemberRepo MemberRepo)
        {
            _repo = repo;
            _bookRepo = bookRepo;
            _hubContext = hubContext;
            _MemberRepo = MemberRepo;


        }
        public bool ApproveRequest(int requestId,string ended)
        {
            var request = _repo.GetById(requestId);
            if (request == null) return false;

            request.ended = ended;
            _repo.updaterequest(request);

            var book = _bookRepo.Getbook(request.bookId);
            if (book != null)
            {
                if (ended == "Approved") book.IsAvilable = false;
                else book.IsAvilable = true; 
                _bookRepo.updateBook(book);
            }
            return true;
        }

        public bool CancelRequest(int requestId, int memberId)
        {
            var request = _repo.GetById(requestId);
            if (request == null || request.memberId != memberId) return false;
            _repo.DeleteRequest(request);
            var book = _bookRepo.Getbook(request.bookId);
            if (book != null)
            {
                book.IsAvilable = true;
                _bookRepo.updateBook(book);
            }
            return true;
        }
        public async Task<bool> CreateRequest(requestDTO dto)
        {
            Console.WriteLine($"Incoming Request: bookId={dto.bookId}, memberId={dto.memberId}, days={dto.days}");

            if (dto.bookId == null || dto.memberId == null || dto.days == null)
                return false;

            var book = _bookRepo.Getbook(dto.bookId.Value);
            if (book == null || !book.IsAvilable)
            {
                Console.WriteLine($"Book ID {dto.bookId} is not available for borrowing.");
                return false;
            }

            var existingRequest = _repo.GetAllRequests()
                .FirstOrDefault(r => r.bookId == dto.bookId && r.memberId == dto.memberId && r.ended == "Pending");

            if (existingRequest != null)
            {
                Console.WriteLine($"Duplicate found → BookId={existingRequest.bookId}, MemberId={existingRequest.memberId}, Ended={existingRequest.ended}");
                return false;
            }

            var request = new requests
            {
                bookId = dto.bookId.Value,
                memberId = dto.memberId.Value,
                days = dto.days.Value,
                ended = "Pending"
            };

            _repo.CreatRequest(request);

            await _hubContext.Clients.All.SendAsync("NewBorrowRequest", new
            {
                request.Id,
                request.bookId,
                request.memberId,
                request.days,
                request.ended
            });
            //var message = new { request.Id, request.memberId, request.bookId, Action = "NewRequest" };


            return true;
            
        }

        public IEnumerable<requestDTO> GetAllRequests()
        {
            var requests = _repo.GetAllRequests();
            var books = _bookRepo.GetBooks();
            var members = _MemberRepo.Getmembers(); 

            return requests.Select(r =>
            {
                var book = books.FirstOrDefault(b => b.Id == r.bookId);
                var member = members.FirstOrDefault(m => m.Id == r.memberId);

                return new requestDTO
                {
                    Id = r.Id,
                    bookId = r.bookId,
                    memberId = r.memberId,
                    days = r.days,
                    ended = r.ended,
                    BookTitle = book?.Title,
                    ImageUrl = book?.ImageUrl,
                    MemberName = member?.Name
                };
            }).ToList();
        }


        public IEnumerable<requestDTO> GetMemberRequests(int memberId)
        {
            var requests = _repo.GetAllRequests()
                .Where(r => r.memberId == memberId)
                .ToList();

            var requestDtos = new List<requestDTO>();

            foreach (var r in requests)
            {
                var book = _bookRepo.Getbook(r.bookId); 

                requestDtos.Add(new requestDTO
                {
                    Id = r.Id,
                    bookId = r.bookId,
                    days = r.days,
                    ended = r.ended,
                    Title = book?.Title,
                    ImageUrl = book?.ImageUrl 
                });
            }

            return requestDtos;
        }
    }
}

using library_mangment.library.Application.dto;
using library_mangment.library.Application.@interface;
using library_mangment.library.domain.entities;
using library_mangment.library.domain.@interface;
using library_mangment.library.Infrastructure.@interface;


namespace library_task.library.Application.services
{
    public class BorrowReturnService : IBorrowReturnService
    {
        private readonly IBookRepo bookRepo;
        private readonly IMemberRepo MemberRepo;
        private readonly IBorrowRecordRepo borrowRecordRepo;
        public BorrowReturnService(IBookRepo bookRepository, IBorrowRecordRepo borrowRecordRepository, IMemberRepo MemberRepo)
        {
            bookRepo = bookRepository;
            this.MemberRepo = MemberRepo;
            borrowRecordRepo = borrowRecordRepository;
        }

        public bool BorrowBook(int memberId, int bookId)
        {
            var member = MemberRepo.GetMemberByID(memberId); 
            if (member == null)
            {
                Console.WriteLine($"Borrow failed: Member with ID {memberId} not found");
                return false;
            }
            var book = bookRepo.Getbook(bookId);

            if (book == null)
                return false;

            if (!book.IsAvilable)
                return false;

            var borrowRecord = new BorrowRecord
            {
                bookId = bookId,
                memberId = memberId,
                BorrowDate = DateTime.UtcNow
            };

            borrowRecordRepo.AddRecord(borrowRecord);

            book.IsAvilable = false;
            bookRepo.updateBook(book);

            return true;
        }

        public bool ReturnBook(int memberId, int bookId)
        {
            var record = borrowRecordRepo.GetId(bookId, memberId);

            if (record == null)
            {
                
                return false;
            }

            record.ReturnDate = DateTime.UtcNow;
            borrowRecordRepo.UpdateRecord(record);

            var book = bookRepo.Getbook(bookId);
            if (book != null)
            {
                book.IsAvilable = true;
                bookRepo.updateBook(book);
            }
            return true;



        }
    }
}

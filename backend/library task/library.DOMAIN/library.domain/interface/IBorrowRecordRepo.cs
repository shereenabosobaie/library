
using library_mangment.library.domain.entities;

namespace library_mangment.library.Infrastructure.@interface
{
    public interface IBorrowRecordRepo
    {
        BorrowRecord? GetId(int bookId,int memberId );
        bool AddRecord(BorrowRecord borec);
        bool UpdateRecord(BorrowRecord borec);


    }
}

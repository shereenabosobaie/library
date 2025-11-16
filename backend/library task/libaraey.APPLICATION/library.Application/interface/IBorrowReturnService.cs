namespace library_mangment.library.Application.@interface
{
    public interface IBorrowReturnService
    {
        bool BorrowBook(int memberId, int bookId);
        bool ReturnBook(int memberId, int bookId);
    }
}

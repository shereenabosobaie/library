using libaraey.APPLICATION.library.Application.dto;
using library_mangment.library.Application.dto;

namespace library_mangment.library.Application.@interface
{
    public interface IBookServies
    {
        ICollection<BookDto> GetAllBooks();
        bool CreateBook(CreateBookDto dto);
        public ICollection<BookDto> recent();
        bool UpdateBook(int id, BookDto dto);
        bool DeleteBook(int id);
        public IEnumerable<BookDto> SearchBooks(string query);
        public BookDto? GetBookById(int id);
    }
}

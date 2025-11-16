using library_mangment.library.domain.entities;


namespace library_mangment.library.Infrastructure.@interface
{
    public interface IBookRepo
    {
        ICollection<book> GetBooks();
        bool creatBook(book book);
        bool updateBook(book book);
        bool bookExists(int id);
        public bool Deletebook(book book);
        public book Getbook(int id);
    }
}

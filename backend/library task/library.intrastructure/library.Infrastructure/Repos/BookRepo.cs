using library_mangment.library.domain.entities;
using library_mangment.library.Infrastructure.appdbcontext;
using library_mangment.library.Infrastructure.@interface;
using Microsoft.EntityFrameworkCore;

namespace library_mangment.library.Infrastructure.Repos
{
    public class BookRepo : IBookRepo
    {
        private readonly Dbcontext context;

        public BookRepo(Dbcontext context)
        {
            this.context = context;
        }

        public bool bookExists(int id)
        {
            return context.books.Any(c => c.Id == id);

        }

        public bool creatBook(book book)
        {
            context.books.Add(book);
            
            return context.SaveChanges()>0?true:false;
        }

        public ICollection<book> GetBooks()
        {
            return context.books.OrderBy(x => x.Id).ToList();
        }

        public bool updateBook(book book)
        {
            context.books.Update(book);

            return context.SaveChanges() > 0 ;

        }
        public book Getbook(int id)
        {
            return context.books.Where(e => e.Id == id).FirstOrDefault();
        }
        public bool Deletebook(book book)
        {
            context.books.Remove(book);
            return context.SaveChanges() > 0 ? true : false;
        }


    }
}

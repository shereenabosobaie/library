using libaraey.APPLICATION.library.Application.dto;
using library_mangment.library.Application.dto;
using library_mangment.library.Application.@interface;
using library_mangment.library.domain.entities;
using library_mangment.library.Infrastructure.@interface;
using library_mangment.library.Infrastructure.Repos;
using System.Net;

namespace library_task.library.Application.services
{
    public class BookService : IBookServies
    {
        private readonly IBookRepo bookRepo;

        public BookService(IBookRepo bookRepo)
        {
            this.bookRepo = bookRepo;
        }

        public bool CreateBook(CreateBookDto bookdto)
        {
            var bookcheck = bookRepo.GetBooks()
                .Where(c => c.Title.Trim().ToUpper() == bookdto.Title.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (bookcheck!=null)
            {
                return false;
            }
            var bookadded = new book
            {                        
                Title = bookdto.Title,
                Author = bookdto.Author,
                PublishYear = bookdto.PublishYear,
                Description = bookdto.Description,       
                ImageUrl = bookdto.ImageUrl,             
                addedtime = DateTime.UtcNow,
            };

            return bookRepo.creatBook(bookadded);
        }
        public BookDto? GetBookById(int id)
        {
            var book = bookRepo.Getbook(id);
            if (book == null)
                return null;

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishYear = book.PublishYear,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                IsAvilable = book.IsAvilable,
                Rate = book.Rate
            };
        }
        public IEnumerable<BookDto> SearchBooks(string query)
        {
            var books = bookRepo.GetBooks();

            return books
                .Where(b => b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                            b.Author.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ImageUrl = b.ImageUrl
                })
                .ToList();
        }
        public bool DeleteBook(int id)
        {
            var bookToDelete = bookRepo.Getbook(id);
            if(bookToDelete==null) {return false;}
            return bookRepo.Deletebook(bookToDelete);
        }

        public ICollection<BookDto> GetAllBooks()
        {
            var books = bookRepo.GetBooks().Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PublishYear = b.PublishYear,
                IsAvilable = b.IsAvilable,
                ImageUrl = b.ImageUrl,
            }).ToList();
            return books;
        }

        public bool UpdateBook(int bookId, BookDto updatedBook)
        {
            var existingBook = bookRepo.Getbook(bookId);
            if (existingBook == null) return false;

            existingBook.Title = updatedBook.Title ?? existingBook.Title;
            existingBook.Author = updatedBook.Author ?? existingBook.Author;
            existingBook.Description = updatedBook.Description ?? existingBook.Description;
            existingBook.PublishYear = updatedBook.PublishYear != 0 ? updatedBook.PublishYear : existingBook.PublishYear;
            existingBook.IsAvilable = updatedBook.IsAvilable;
            existingBook.Rate = updatedBook.Rate != 0 ? updatedBook.Rate : existingBook.Rate;
            existingBook.addedtime = existingBook.addedtime == default ? DateTime.Now : existingBook.addedtime;

            if (!string.IsNullOrEmpty(updatedBook.ImageUrl))
                existingBook.ImageUrl = updatedBook.ImageUrl;

            bookRepo.updateBook(existingBook);
            return true;
        }

        public ICollection<BookDto> recent() 
        {
            var books = bookRepo.GetBooks()
            .OrderByDescending(b => b.addedtime)
            .Take(10)
            .Select(b => new BookDto
             {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PublishYear = b.PublishYear,
                Description = b.Description,
                IsAvilable = b.IsAvilable,
                ImageUrl = b.ImageUrl,
                addedtime = b.addedtime
            })
            .ToList();

            return books;
        }
    }


}

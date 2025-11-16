using library.DOMAIN.library.domain;
using library.DOMAIN.library.domain.entities;
using library_mangment.library.domain.entities;
using library_mangment.library.Infrastructure.appdbcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.intrastructure.library.Infrastructure.Repos
{
    public class rateRepo : IrateRepo
    {
        private readonly Dbcontext _context;
        public rateRepo(Dbcontext context) => _context = context;

        public bool AddRate(rate rate)
        {
            _context.BookRates.Add(rate);
            return _context.SaveChanges() > 0;
        }

        public double GetAverageRating(int bookId)
        {
            var rates = _context.BookRates.Where(r => r.bookId == bookId);
            return rates.Any() ? rates.Average(r => r.Rate) : 0;
        }

        public IEnumerable<rate> GetBookRates(int bookId)
        {
            return _context.BookRates.Where(r => r.bookId == bookId).ToList();
        }

        public rate? GetRate(int memberId, int bookId)
        {
            return _context.BookRates.FirstOrDefault(r => r.memberId == memberId && r.bookId == bookId);
        }
    }
}

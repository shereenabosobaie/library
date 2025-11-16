using library.DOMAIN.library.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.DOMAIN.library.domain 
{
    public interface IrateRepo
    {
        bool AddRate(rate rate);
        rate? GetRate(int memberId, int bookId);
        IEnumerable<rate> GetBookRates(int bookId);
        double GetAverageRating(int bookId);
    }
}

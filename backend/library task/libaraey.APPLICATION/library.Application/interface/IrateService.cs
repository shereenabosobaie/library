using libaraey.APPLICATION.library.Application.dto;
using library.DOMAIN.library.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libaraey.APPLICATION.library.Application
{
    public interface  IrateService
    {
        bool AddRate(ratedto dto);
        double GetAverageRating(int bookId);
        IEnumerable<rate> GetBookRates(int bookId);
        rate? GetRate(int memberId, int bookId);
    }
}

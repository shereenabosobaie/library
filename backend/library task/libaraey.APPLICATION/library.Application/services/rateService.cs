using libaraey.APPLICATION.library.Application.dto;
using library.DOMAIN.library.domain;
using library.DOMAIN.library.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libaraey.APPLICATION.library.Application.services
{
    public class rateService : IrateService
    {
        private readonly IrateRepo _repo;

        public rateService(IrateRepo repo)
        {
            _repo = repo;
        }

        public bool AddRate(ratedto dto)
        {
            
            var existingRate = _repo.GetRate(dto.MemberId, dto.BookId);
            if (existingRate != null)
            {
                
                existingRate.Rate = dto.Rate;
                return _repo.AddRate(existingRate); 
            }

            var rate = new rate
            {
                bookId = dto.BookId,
                memberId = dto.MemberId,
                Rate = dto.Rate
            };

            return _repo.AddRate(rate);
        }

        public double GetAverageRating(int bookId)
        {
            return _repo.GetAverageRating(bookId);
        }

        public IEnumerable<rate> GetBookRates(int bookId)
        {
            return _repo.GetBookRates(bookId);
        }

        public rate? GetRate(int memberId, int bookId)
        {
            return _repo.GetRate(memberId, bookId);
        }
    }
}

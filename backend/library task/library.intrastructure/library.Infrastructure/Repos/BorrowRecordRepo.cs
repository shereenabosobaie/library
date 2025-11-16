
using library_mangment.library.domain.entities;
using library_mangment.library.Infrastructure.appdbcontext;
using library_mangment.library.Infrastructure.@interface;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace library_mangment.library.Infrastructure.Repos
{
    public class BorrowRecordRepo : IBorrowRecordRepo
    {

        private readonly Dbcontext context;

        public BorrowRecordRepo(Dbcontext context)
        {
            this.context = context;
        }

        public bool AddRecord(BorrowRecord borec)
        {
            context.Add(borec);
            return context.SaveChanges() > 0;
        }

        public BorrowRecord? GetId(int bookId, int memberId)
        {
            return context.borrowRecords
               .FirstOrDefault(r => r.bookId == bookId&&r.memberId==memberId && r.ReturnDate==null);
        }

        public bool UpdateRecord(BorrowRecord borec)
        {
            context.Update(borec);
            return context.SaveChanges() > 0;
        }
    }
}

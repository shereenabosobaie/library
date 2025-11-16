using library_mangment.library.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library.DOMAIN.library.domain.entities
{
    public class rate
    {
        public int Id { get; set; }
        public int bookId { get; set; }
        public book Book { get; set; }
        public int memberId { get; set; }
        public member Member { get; set; }

        public double Rate { get; set; }
    }
}

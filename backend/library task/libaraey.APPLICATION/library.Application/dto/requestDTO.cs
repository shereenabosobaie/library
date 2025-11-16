using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libaraey.APPLICATION.library.Application.dto
{
    public class requestDTO
    {
        public int? Id { get; set; }           
        public int? bookId { get; set; }       
        public int? memberId { get; set; }  
        public string? Title { get; set; }
        public int? days {  get; set; }
        public string? ImageUrl { get; set; }
        public string? ended { get; set; }
        public string? BookTitle { get; set; }
        public string? MemberName { get; set; }
    }
}

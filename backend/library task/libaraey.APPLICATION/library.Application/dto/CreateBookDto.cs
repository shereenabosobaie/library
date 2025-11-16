using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libaraey.APPLICATION.library.Application.dto
{
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public int PublishYear { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
    }
}

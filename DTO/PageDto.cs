using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.DTO
{
    public class PageDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Author { get; set; }
    }
}

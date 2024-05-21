using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models.EF
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Stars { get; set; }
        public string ImageName { get; set; }
        public string UserName { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}

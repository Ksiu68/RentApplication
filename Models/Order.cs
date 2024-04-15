using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int AppartamentId { get; set; }
        public int CustomerId { get; set; }
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Status { get; set; }
    }
}

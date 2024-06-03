using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public int AppartamentId { get; set; }
        public string CustomerId { get; set; }
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Status { get; set; }
        [ForeignKey("OwnerId")]
        public User Owner {get;set;}
        [ForeignKey("CustomerId")]
        public User Customer {get;set;}
        [ForeignKey("AppartamentId")]
        public Appartament Appartament {get;set;}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class Appartament
    {
        [Key]
        public int Id { get; set; }
        public int HouseId { get; set; }
        public int OwnerId { get; set; }
        public string Description { get; set; }
        public string Balcony { get; set; }
        public string Repair { get; set; }
        public string Wifi { get; set; }
        public int Area { get; set; }
        public int CountOfRooms { get; set; }
        public int Floor { get; set; }
        public decimal Price { get; set; }
    }
}

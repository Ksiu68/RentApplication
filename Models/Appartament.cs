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
        public string Type {get;set;}
        public string ReferenceTo3D {get;set;}
        public int countOfBedrooms {get;set;}
        public int CountOfRooms { get; set; }
        public int Area { get; set;}
        public int Floor { get; set; }
        public decimal Price { get; set; }
    }
}

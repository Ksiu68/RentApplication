using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class Appartament
    {
        [Key]
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public int TypeId {get;set;}
        public string ReferenceTo3D {get;set;}
        public int countOfBedrooms {get;set;}
        public int CountOfRooms { get; set; }
        public int Area { get; set;}
        public int Floor { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("PlaceId")]
        public Place Place {get;set;}
        [ForeignKey("UserId")]
        public User User{get;set;}
        [ForeignKey("TypeId")]
        public Type Type {get;set;}
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    }
}

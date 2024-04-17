using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class House
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; }
        public string Elevator { get; set; }
        public string Interom { get; set; }
        public string Parking { get; set; }
        public string Playground { get; set; }
        public string Area { get; set; }
        public string NumberOfFloors { get; set; }
        public string YearOfConstruction{get;set;}
        public string Metro {get;set;}
        public string DistanceToMetro{get;set;}
        public int NumberHouse { get; set; }
    }
}

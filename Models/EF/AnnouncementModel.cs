﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models.EF
{
    public class AnnouncementModel{
        [Required(ErrorMessage ="NumberHouse is required")]
        public int NumberHouse { get; set; }
        public string Playgroung { get; set; }
        public string Parking { get; set; }
        public string Interom { get; set; }
        public string Elevator { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public int Floor { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string Balcony { get; set; }
        public string Repair { get; set; }

        [Required(ErrorMessage = "Area is required")]
        public int Area { get; set; }

        [Required(ErrorMessage = "Count Of Rooms is required")]
        public int CountOfRooms { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        public string Wifi { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string ImageBase64 { get; set; }
        
        public string ImageName { get; set; }
    }
}
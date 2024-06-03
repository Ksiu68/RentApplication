using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int AppartamentId {get;set;}
        [ForeignKey("AppartamentId")]
        public Appartament Appartament{get;set;}
    }
}

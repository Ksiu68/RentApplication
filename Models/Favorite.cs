using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class Favorite
    {
        [Key]
        public int Id{get;set;}
        public string UserId { get; set; }
        public int AppartamentId { get; set; }
        [ForeignKey("AppartamentId")]
        public Appartament Appartament{get;set;}
        [ForeignKey("UserId")]
        public User User {get;set;}
    }
}

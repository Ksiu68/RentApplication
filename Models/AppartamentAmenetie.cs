using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class AppartamentAmenetie
    {
        [Key]
        public int Id{get;set;}
        public int AppartamentId { get; set; }  
        public int AmenetieId { get; set; }
        [ForeignKey("AppartamentId")]
        public Appartament Appartament{get;set;}
        [ForeignKey("AmenetieId")]
        public Amenetie Amenetie {get;set;}
    }
}

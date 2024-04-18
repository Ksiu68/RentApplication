using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class AppartamentAmenetie
    {
        [Key]
        public int AppartamentId { get; set; }
        [Key]
        public int AmenetieId { get; set; }
    }
}

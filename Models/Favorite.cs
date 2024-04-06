using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class Favorite
    {
        [Key]
        public int CustomerId { get; set; }
        public int AppartamentId { get; set; }
    }
}

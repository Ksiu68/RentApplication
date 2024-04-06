using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class Owner
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
    }
}

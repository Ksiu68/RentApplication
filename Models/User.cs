using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Models
{
    public class User : IdentityUser
    {
        public string LastName { get; set; }
        public string ImagePath { get; set; }
    }

}

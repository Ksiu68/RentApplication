using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pomelo.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RentApplication.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Amenetie> Ameneties { get; set; }
        public DbSet<Type> Types {get;set;}
        public DbSet<AppartamentAmenetie> AppartamentAmeneties { get; set; }
        public DbSet<Appartament> Appartaments { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

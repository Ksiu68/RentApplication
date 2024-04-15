using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApplication.Models;
using RentApplication.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RentApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly UserManager<User> userManager;
        private ApplicationContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OwnerController(ApplicationContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            db = context;
            this.userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("getAnnouncements")]
        public async Task<IActionResult> getAnnouncementsAsync()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await userManager.FindByNameAsync(userName);
            Owner owner = db.Owners.Where(o => o.UserId.Equals(user.Id)).First();
            List<Appartament> appartaments = db.Appartaments.Where(a => a.OwnerId == owner.Id).ToList();
            return Ok(appartaments);
        }

        [HttpPost("{appartamentId}")]
        [Route("removeAnnouncement")]
        public async Task<IActionResult> removeAnnouncement(int appartamentId)
        {
            Appartament appartament = db.Appartaments.FindAsync(appartamentId).Result;
            House house = db.Houses.FindAsync(appartament.HouseId).Result;
            Owner owner = db.Owners.FindAsync(appartament.OwnerId).Result;
            Image image = db.Images.FindAsync(appartament.ImageId).Result;

            if(db.Appartaments.Where(a => a.OwnerId == appartament.OwnerId).Count() == 1)
            {
                db.Owners.Remove(owner);
                await db.SaveChangesAsync();
            }

            db.Houses.Remove(house);
            await db.SaveChangesAsync();
            db.Images.Remove(image);
            await db.SaveChangesAsync();

            if (appartament == null)
            {
                return BadRequest(new Response { Status = "Bad", Message = "Announcement not find!" });
            }

            db.Appartaments.Remove(appartament);
            await db.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "Announcement was removed successfully!" });
        }
    }
}

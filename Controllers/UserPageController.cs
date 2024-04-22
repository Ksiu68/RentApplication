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
    [Route("api/[controller]")]
    [ApiController]
    public class UserPageController : Controller
    {

        private readonly UserManager<User> userManager;
        private ApplicationContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserPageController(ApplicationContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            db = context;
            this.userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize]
        [HttpPost("{apartamentId}")]
        [Route("addToFavorite")]
        public async Task<IActionResult> addToFavorite(int appartamentId)
        {
            Appartament appartament = db.Appartaments.FindAsync(appartamentId).Result;
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await userManager.FindByNameAsync(userName);
            int userId;
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);

            // Если запись существует, возвращаем её Id
            if (customer != null)
            {
                userId = customer.Id;
            }
            else
            {
                // Если запись не существует, создаем новую запись
                var newCustomer = new Customer { UserId = user.Id };
                db.Customers.Add(newCustomer);
                await db.SaveChangesAsync();
                userId = newCustomer.Id;
            }
            Favorite favoriteAppartament = new Favorite()
            {
                CustomerId = userId,
                AppartamentId = appartament.Id
            };
            db.Favorites.Add(favoriteAppartament);
            await db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Added to favorite successfully!" });
        }
        [Authorize]
        [HttpPost("{apartamentId}")]
        [Route("removeToFavorite")]
        public async Task<IActionResult> removeFromFavorite(int appartamentId)
        {
            Favorite favoriteAppartament = db.Favorites
                .Where(f => f.AppartamentId == appartamentId)
                .FirstOrDefault();
            db.Favorites.Remove(favoriteAppartament);
            await db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Removed from favorite successfully!" });
        }

    }
}

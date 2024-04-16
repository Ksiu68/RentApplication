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
    public class RentController : Controller
    {
        private readonly UserManager<User> userManager;
        private ApplicationContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RentController(ApplicationContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            db = context;
            this.userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("{apartamentId}")]
        [Route("makeRent")]
        public async Task<IActionResult> makeRent(int appartamentId, [FromBody] Calendar calendar)
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
            Order order = new Order()
            {
                CustomerId = userId,
                OwnerId = appartament.OwnerId,
                AppartamentId = appartament.Id,
                Price = getPrice(calendar, appartament.Price),
                Date = DateTime.Now,
                DateStart = calendar.DateStart,
                DateEnd = calendar.DateEnd,
                Status = "Created"
            };
            db.Orders.Add(order);
            await db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Order was created successfully!" });
        }

        private decimal getPrice(Calendar calendar, decimal pricePerDay)
        {
            if (!calendar.DateStart.HasValue || !calendar.DateEnd.HasValue)
            {
                throw new ArgumentException("DateStart and DateEnd must have a value.");
            }

            // Вычисляем разницу в днях между DateStart и DateEnd
            int numberOfDays = (calendar.DateEnd.Value - calendar.DateStart.Value).Days;

            // Вычисляем конечную стоимость
            decimal totalPrice = pricePerDay * numberOfDays;

            return totalPrice;
        }
    }
}

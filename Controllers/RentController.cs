using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
        [Authorize]
        [HttpPost]
        [Route("makeRent/{appartamentId}")]
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

        [Authorize]
        [HttpGet]
        [Route("getRents")]
        public async Task<IActionResult> getRents()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await userManager.FindByNameAsync(userName);
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
            List<Order> orders = db.Orders.Where(o => o.CustomerId == customer.Id).ToList();
            return Ok(orders);
        }

        [Authorize]
        [HttpPost]
        [Route("makeReview")]
        public async Task<IActionResult> makeReview([FromBody] Review review)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await userManager.FindByNameAsync(userName);
            int userId;
            var customer = await db.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
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
            var order = db.Orders.Where(o => o.AppartamentId == review.AppartamentId && o.CustomerId == userId).FirstOrDefault();
            if(order == null) { 
                return BadRequest(new Response { Status = "Bad", Message = "You didn't rent this appartament!" });
            }
            review.CustomerId = userId;
            db.Reviews.Add(review);
            await db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Review was add successfully!" });
        }

        [HttpGet]
        [Route("getReviews/{appId}")]
        public IActionResult getReviews(int appId)
        {
            List<ReviewDTO> reviewDTOs = new List<ReviewDTO>();
            List<Review> reviews = db.Reviews.Where(r => r.AppartamentId == appId).ToList();
            foreach(Review review in reviews)
            {
                Order order = db.Orders.Where(o => o.CustomerId == review.CustomerId && o.AppartamentId == review.AppartamentId).FirstOrDefault();
                Customer customer = db.Customers.Where(c => c.Id == review.CustomerId).FirstOrDefault();
                User user = db.Users.Where(u => u.Id == customer.UserId).FirstOrDefault();
                ReviewDTO reviewDTO = new ReviewDTO()
                {
                    Id = review.Id,
                    Text = review.Text,
                    Stars = review.Stars,
                    DateStart = order.DateStart,
                    DateEnd = order.DateEnd,
                    UserName = user.UserName,
                    ImageName = user.ImagePath.Replace("uploads\\", "")
                };
                reviewDTOs.Add(reviewDTO);
            }
            return Ok(reviewDTOs);
        }

        [HttpPost]
        [Route("rejectRent/{orderId}")]
        public IActionResult rejectRent(int orderId)
        {
            Order order = db.Orders
                .Where(o => o.Id == orderId && o.Status == "Created")
                .FirstOrDefault();
            DateTime dateOrder = order.DateStart ?? DateTime.Now; 
            DateTime currentDate = DateTime.Now; 

            TimeSpan diff = dateOrder - currentDate;
            if(diff.TotalHours <= 24){
                return BadRequest(new Response { Status = "Fail", Message = "Time is less than 24 hours!" });
            }
            order.Status = "Rejected";
            order.Date = DateTime.Now;
            db.Orders.Update(order);
            db.SaveChanges();
            return Ok(new Response { Status = "Success", Message = "You rejected order!" });
        }

        [HttpPost]
        [Route("DoneRent/{orderId}")]
        public IActionResult doneRent(int orderId)
        {
            Order order = db.Orders
                .Where(o => o.Id == orderId && o.Status == "Created")
                .FirstOrDefault();
            DateTime dateOrder = order.DateEnd ?? DateTime.Now; 
            DateTime currentDate = DateTime.Now; 

            TimeSpan diff = currentDate - dateOrder;
            if(diff.TotalHours < 0){
                return BadRequest(new Response { Status = "Fail", Message = "You can't done yet this order!" });
            }
            order.Status = "Done";
            order.Date = DateTime.Now;
            db.Orders.Update(order);
            db.SaveChanges();
            return Ok(new Response { Status = "Success", Message = "You're done order!" });
        }

        [HttpGet]
        [Route("getBusyDates/{appId}")]
        public IActionResult getBusyDates(int appId)
        {
            var activeOrders = db.Orders
                .Where(o1 => o1.Status == "Created" && o1.AppartamentId == appId)
                .Select(o => new
                {
                    DateStart = o.DateStart,
                    DateEnd = o.DateEnd
                })
                .ToList();
            return Ok(activeOrders);
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

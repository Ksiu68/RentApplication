using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApplication.Models;
using RentApplication.Models.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        [HttpPost]
        [Route("addToFavorite/{appartamentId}")]
        public async Task<IActionResult> addToFavorite(int appartamentId)
        {
            Appartament appartament = db.Appartaments.FindAsync(appartamentId).Result;
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await userManager.FindByNameAsync(userName);
            IdentityResult identityResult = await userManager.AddToRoleAsync(user, UserRoles.Customer);
        //    int userId;
        //    var customer = await db.Users.FirstOrDefaultAsync(c => c.UserId == user.Id);
            
            // Если запись существует, возвращаем её Id
            // if (customer != null)
            // {
            //     userId = customer.Id;
            // }
            // else
            // {
            //     // Если запись не существует, создаем новую запись
            //     var newCustomer = new Customer { UserId = user.Id };
            //     db.Customers.Add(newCustomer);
            //     await db.SaveChangesAsync();
            //     userId = newCustomer.Id;
            // }
            Favorite favoriteAppartament = new Favorite()
            {
                UserId = user.Id,
                AppartamentId = appartament.Id
            };
            db.Favorites.Add(favoriteAppartament);
            await db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Added to favorite successfully!" });
        }
        [Authorize]
        [HttpPost]
        [Route("removeToFavorite/{appartamentId}")]
        public async Task<IActionResult> removeFromFavorite(int appartamentId)
        {
            Favorite favoriteAppartament = db.Favorites
                .Where(f => f.AppartamentId == appartamentId)
                .FirstOrDefault();
            db.Favorites.Remove(favoriteAppartament);
            await db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Removed from favorite successfully!" });
        }
        [HttpGet("role")]
        [Authorize]
        public IActionResult GetUserRole()
        {
            var userClaims = User.Claims;

            var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (roleClaim != null)
            {
                return Ok(new { Role = roleClaim.Value });
            }

            return NotFound("Role not found for the user.");
        }
        [Authorize]
        [HttpGet]
        [Route("getFavorites")]
        public async Task<IActionResult> getFavorites()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var userId = await userManager.FindByNameAsync(userName);
            User user = db.Users.Where(u => u.Id == userId.Id).FirstOrDefault();
            List<Favorite> favorites = db.Favorites.Where(f => f.UserId == user.Id).ToList();
            //List<AppartamentDTO> appartamentDTOs = new List<AppartamentDTO>();
            //foreach (Favorite favorite in favorites)
            //{
            //    using (HttpClient client = new HttpClient())
            //    {
            //        HttpResponseMessage response = await client.GetAsync("http://localhost:11732/api/appartament/getAppartament/" + favorite.AppartamentId);

            //        if (response.IsSuccessStatusCode)
            //        {
            //            appartamentDTOs.Add(response.Content.ReadAsStringAsync());
            //        }
            //    }
            //}
            return Ok(favorites);
        }

        [Authorize]
        [HttpGet]
        [Route("getUserInfo")]
        public async Task<IActionResult> getUserInfo()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var userId = await userManager.FindByNameAsync(userName);
            User user = db.Users.Where(u => u.Id == userId.Id).FirstOrDefault();
            UserDTO userDTO = new UserDTO(){
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                FIO = user.FIO,
                ImageName = user.ImagePath.Replace("uploads\\", ""),
                Username = user.UserName
            };
            //IdentityResult identityResult = await userManager.AddToRoleAsync(user, UserRoles.Admin);

            return Ok(userDTO);
        }

        [Authorize]
        [HttpPost]
        [Route("updatePhoto")]
        public async Task<IActionResult> updatePhoto([FromBody] ImageDTO imageDTO)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var userId = await userManager.FindByNameAsync(userName);
            string imagePath = await createImageAsync(imageDTO.ImageBase64, imageDTO.ImageName);
            if(imagePath.Equals("Fail")) return BadRequest(new Response { Status = "Bad", Message = "Image was not uploaded!" });
            // Image image = new Image()
            // {
            //     ImagePath = imagePath
            // };
            //var imageResult = await db.Images.AddAsync(image);
            //await db.SaveChangesAsync();
            //if (imageResult == null) return BadRequest(new Response { Status = "Bad", Message = "Image was not created!" });
            User user = db.Users.Where(u => u.Id == userId.Id).FirstOrDefault();
            user.ImagePath = imagePath;
            var userResult = db.Users.Update(user);
            await db.SaveChangesAsync();
            if (userResult == null) return BadRequest(new Response { Status = "Bad", Message = "Image was not created!" });
            return Ok(new Response { Status = "Success", Message = "Photo was updated!" });
        }

         public async Task<string> createImageAsync(string imageBase64, string imageName)
        {
            string base64String = imageBase64;
            try
            {
                // Преобразуем base64 строку в байтовый массив
                byte[] imageBytes = Convert.FromBase64String(base64String);


                // Пример сохранения изображения в файл
                string uploadsFolder = Path.Combine(_httpContextAccessor.HttpContext.Request.PathBase.Value, "uploads");
                string imagePath = Path.Combine(uploadsFolder, imageName); // Путь для сохранения изображения
                using (FileStream fs = new FileStream(imagePath, FileMode.Create))
                {
                    // Записываем байты изображения в файл
                    await fs.WriteAsync(imageBytes, 0, imageBytes.Length);
                }
                return imagePath;
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, возвращаем ошибку сервера
                return "Fail";
            }
        }
    }
}

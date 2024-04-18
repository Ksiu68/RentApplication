using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentApplication.Models;
using RentApplication.Models.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RentApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppartamentController : Controller
    {
        private Dictionary<int, string> ameneties = new Dictionary<int, string>(){
            { 1, "Посудомоечная машина" },
            { 2, "Интернет" },
            { 3, "Телевизор" },
            { 4, "Можно курить"},
            { 5, "Стиральная машина"},
            { 6, "Микроволновка"},
            { 7, "Кондиционер"},
            { 8, "Можно с детьми"},
            { 9, "Можно с животными"}
        };
        private readonly UserManager<User> userManager;
        private ApplicationContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppartamentController(ApplicationContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            db = context;
            this.userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost]
        [Route("make")]
        public async Task<IActionResult> makeAnnouncement([FromBody] AnnouncementModel model)
        {
            string imagePath = await createImageAsync(model.ImageBase64, model.ImageName);
            if(imagePath.Equals("Fail")) return BadRequest(new Response { Status = "Bad", Message = "Image was not uploaded!" });
            Image image = new Image()
            {
                ImagePath = imagePath
            };
            var imageResult = await db.Images.AddAsync(image);
            await db.SaveChangesAsync();
            if (imageResult == null) return BadRequest(new Response { Status = "Bad", Message = "Image was not created!" });
            House house = new House()
            {
                Address = model.Address,
                Elevator = model.Elevator,
                Interom = model.Interom,
                Parking = model.Parking,
                Playground = model.Playgroung,
                NumberHouse = model.NumberHouse
            };
            var houseResult = await db.Houses.AddAsync(house);
            await db.SaveChangesAsync();
            if (houseResult == null) return BadRequest(new Response { Status = "Bad", Message = "House was not created!" });
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await userManager.FindByNameAsync(userName);
            Owner owner = null;
            if (db.Owners.Where(o => o.UserId.Equals(user.Id)).Count() != 1)
            {
                owner = new Owner()
                {
                    UserId = user.Id
                };
                var ownerResult = await db.Owners.AddAsync(owner);
                await db.SaveChangesAsync();
                if (ownerResult == null) return BadRequest(new Response { Status = "Bad", Message = "Owner was not created!" });
            }
            else
            {
                owner = db.Owners.FirstOrDefault(o => o.UserId.Equals(user.Id));
            }
            Appartament appartament = new Appartament()
            {
                Description = model.Description,
                Area = model.Area,
                CountOfRooms = model.CountOfRooms,
                Floor = model.Floor,
                Price = model.Price,
                HouseId = house.Id,
                OwnerId = owner.Id

            };
            var appartamentResult = await db.Appartaments.AddAsync(appartament);
            await db.SaveChangesAsync();
            ImageAppartament imageAppartament = new ImageAppartament()
            {
                AppartamentId = appartament.Id,
                ImageId = image.Id
            };
            var ImageResult = await db.ImageAppartaments.AddAsync(imageAppartament);
            await db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Announcement created successfully!" });

        }
        [HttpGet]
        [Route("getAppartaments")]
        public IActionResult getAppartaments()
        {
            List<Appartament> appartaments = db.Appartaments.ToList();
            return Ok(appartaments);
        }

        [HttpGet]
        [Route("getAppartament")]
        public IActionResult getAppartament(int appartamentId)
        {
            Appartament appartament = db.Appartaments
                .Where(a => a.Id == appartamentId)
                .FirstOrDefault();

            return Ok(appartament);
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
        [HttpGet("getimage/{imageName}")]
        public async Task<IActionResult> GetImage(string imageName)
        {
            string uploadsFolder = Path.Combine(_httpContextAccessor.HttpContext.Request.PathBase.Value, "uploads");
            string imagePath = Path.Combine(uploadsFolder, imageName);

            return await GetImageBytesAsync(imagePath);
        }
        public async Task<IActionResult> GetImageBytesAsync(string imagePath)
        {
            try
            {
                // Проверяем, существует ли файл изображения
                if (!System.IO.File.Exists(imagePath))
                {
                    // Если файл не существует, возвращаем null или выбрасываем исключение
                    // В зависимости от ваших потребностей
                    return null;
                }

                byte[] imageBytes;
                using (FileStream fs = new FileStream(imagePath, FileMode.Open))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await fs.CopyToAsync(ms);
                        imageBytes = ms.ToArray();
                    }
                }

                // Возвращаем изображение в виде массива байтов
                return new FileContentResult(imageBytes, "image/jpeg"); // Или другой MIME-тип, в зависимости от типа изображения
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, возвращаем ошибку сервера
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

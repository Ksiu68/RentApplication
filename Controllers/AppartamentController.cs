using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        private Dictionary<int, string> types = new Dictionary<int, string>(){
            { 1, "Комната" },
            { 2, "Квартира" },
            { 3, "Дом" }
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
        [Authorize]
        [HttpPost]
        [Route("make")]
        public async Task<IActionResult> makeAnnouncement([FromBody] AnnouncementModel model)
        {
            Place house = new Place()
            {
                Address = model.Address,
                Elevator = model.Elevator,
                Interom = model.Interom,
                Parking = model.Parking,
                Playground = model.Playgroung,
                NumberHouse = model.NumberHouse,
                YearOfConstruction = model.YearOfConstruction,
                Area = model.HouseArea,
                NumberOfFloors = model.NumberOfFloors,
                Metro = model.Metro,
                DistanceToMetro = model.DistanceToMetro
            };
            var houseResult = await db.Places.AddAsync(house);
            await db.SaveChangesAsync();
            if (houseResult == null) return BadRequest(new Response { Status = "Bad", Message = "House was not created!" });
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await userManager.FindByNameAsync(userName);
            IdentityResult identityResult = await userManager.AddToRoleAsync(user, UserRoles.Owner);
            // Owner owner = null;
            // if (db.Owners.Where(o => o.UserId.Equals(user.Id)).Count() != 1)
            // {
            //     owner = new Owner()
            //     {
            //         UserId = user.Id
            //     };
            //     var ownerResult = await db.Owners.AddAsync(owner);
            //     await db.SaveChangesAsync();
            //     if (ownerResult == null) return BadRequest(new Response { Status = "Bad", Message = "Owner was not created!" });
            // }
            // else
            // {
            //     owner = db.Owners.FirstOrDefault(o => o.UserId.Equals(user.Id));
            // }
            Models.Type type = db.Types.Where(t => t.Name.Equals(model.Type)).FirstOrDefault();
            if(type == null) return BadRequest(new Response { Status = "Bad", Message = "Bad Type" });
            Appartament appartament = new Appartament()
            {
                Description = model.Description,
                Area = model.Area,
                CountOfRooms = model.CountOfRooms,
                Floor = model.Floor,
                Price = model.Price,
                PlaceId = house.Id,
                UserId = user.Id,
                TypeId = type.Id,
                ReferenceTo3D = model.ReferenceTo3D,
                countOfBedrooms = model.CountOfBedrooms
            };
            var appartamentResult = await db.Appartaments.AddAsync(appartament);
            await db.SaveChangesAsync();
            foreach(ImageDTO imageDTO in model.imageDTOs){
                string imagePath = await createImageAsync(imageDTO.ImageBase64, imageDTO.ImageName);
                if(imagePath.Equals("Fail")) return BadRequest(new Response { Status = "Bad", Message = "Image was not uploaded!" });
                Image image = new Image()
                {
                    ImagePath = imagePath,
                    AppartamentId = appartament.Id
                };
                var imageResult = await db.Images.AddAsync(image);
                await db.SaveChangesAsync();
                if (imageResult == null) return BadRequest(new Response { Status = "Bad", Message = "Image was not created!" });
                //  ImageAppartament imageAppartament = new ImageAppartament()
                // {
                //     AppartamentId = appartament.Id,
                //     ImageId = image.Id
                // };
                // var ImageResult = await db.ImageAppartaments.AddAsync(imageAppartament);
                // await db.SaveChangesAsync();
                // if (ImageResult == null) return BadRequest(new Response { Status = "Bad", Message = "Image was not created!" });
            }
            foreach(string amenetie in model.Amenities){
                 AppartamentAmenetie appartamentAmenetie = new AppartamentAmenetie()
                {
                    AppartamentId = appartament.Id,
                    AmenetieId = ameneties.FirstOrDefault(x => x.Value.Equals(amenetie)).Key
                };
                var amenetieResult = await db.AppartamentAmeneties.AddAsync(appartamentAmenetie);
                if (amenetieResult == null) return BadRequest(new Response { Status = "Bad", Message = "Amenetie was not created!" });
                await db.SaveChangesAsync();
            }
            return Ok(new Response { Status = "Success", Message = "Announcement created successfully!" });

        }
        [HttpGet]
        [Route("getAppartaments")]
        public IActionResult getAppartaments(int? countOfRooms = null, decimal? maxPrice = null, decimal? minPrice = null,
            string? metro =null, string? area = null, string? amenetie = null,
            string? parking = null, string? elevator = null, string? playground = null, 
            string? houseArea = null, string? yearOfConstruction = null)
        {
            IQueryable<Appartament> query = db.Appartaments;

            // Применяем фильтры, если они указаны
            if (countOfRooms.HasValue)
            {
                query = query.Where(a => a.CountOfRooms == countOfRooms);
            }
            if (minPrice.HasValue)
            {
                query = query.Where(a => a.Price >= minPrice);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(a => a.Price <= maxPrice);
            }
           if (!string.IsNullOrEmpty(amenetie))
            {
                var amenetiesList = amenetie.Split(',').Select(int.Parse).ToList();
                var appartamentIdsWithAllAmenities = db.AppartamentAmeneties
                    .Where(aa => amenetiesList.Contains(aa.AmenetieId))
                    .GroupBy(aa => aa.AppartamentId)
                    .Where(g => g.Count() == amenetiesList.Count)
                    .Select(g => g.Key)
                    .ToList();

                query = query.Where(a => appartamentIdsWithAllAmenities.Contains(a.Id));
            }
            if (!string.IsNullOrEmpty(parking))
            {
                query = query.Where(a => a.Place.Parking == parking);
            }
            if (!string.IsNullOrEmpty(elevator))
            {
                query = query.Where(a => a.Place.Elevator == elevator);
            }
            if (!string.IsNullOrEmpty(playground))
            {
                query = query.Where(a => a.Place.Playground == playground);
            }
            if (!string.IsNullOrEmpty(yearOfConstruction))
            {
                query = query.Where(a => a.Place.YearOfConstruction == yearOfConstruction);
            }
            if (!string.IsNullOrEmpty(houseArea))
            {
                query = query.Where(a => a.Place.Area == houseArea);
            }

            // Применяем фильтры по метро и району
            if (!string.IsNullOrEmpty(metro))
            {
                var metroList = metro.Split(',');
                query = query.Where(a => metroList.Contains(a.Place.Metro));
            }
            if (!string.IsNullOrEmpty(area))
            {
                var areaList = area.Split(',');
                query = query.Where(a => areaList.Contains(a.Place.Area));
            }
            List<AppartamentDTO> appartamentDTOs = new List<AppartamentDTO>();
            List<Appartament> appartaments = query.ToList();
            foreach(Appartament appartament in appartaments){
                Place house = db.Places
                    .Where(h => h.Id == appartament.PlaceId)
                    .FirstOrDefault();
                
                if (house == null)
                {
                    return StatusCode(500, "House data not found."); // Вернуть 500 Internal Server Error, если данные о доме не найдены
                }
                User owner = db.Users
                        .Where(o => o.Id == appartament.UserId)
                        .FirstOrDefault();
                if (owner == null)
                {
                    return StatusCode(500, "Owner data not found."); // Вернуть 500 Internal Server Error, если данные о доме не найдены
                }
                // User user = db.Users
                //         .Where(u => u.Id == owner.UserId)
                //         .FirstOrDefault();
                // if (user == null)
                // {
                //     return StatusCode(500, "User data not found."); // Вернуть 500 Internal Server Error, если данные о доме не найдены
                // }
                List<Image> imageAppartament = db.Images
                        .Where(i => i.AppartamentId == appartament.Id)
                        .ToList();
                List<string> imageNames = db.Images
                        .Where(image => imageAppartament.Select(i => i.Id).Contains(image.Id))
                        .Select(image => image.ImagePath.Replace("uploads\\", ""))
                        .ToList();
                List<AppartamentAmenetie> appartamentAmeneties = db.AppartamentAmeneties
                        .Where(amenetie => amenetie.AppartamentId == appartament.Id).ToList();
                List<string> ameneties = db.Ameneties
                        .Where(a => appartamentAmeneties.Select(aa => aa.AmenetieId).Contains(a.Id))
                        .Select(a => a.Name).ToList();
                AppartamentDTO appartamentDTO = new AppartamentDTO(appartament, house, owner, imageNames, ameneties);
                appartamentDTOs.Add(appartamentDTO);
            }
            return Ok(appartamentDTOs);
        }

        [HttpGet]
        [Route("searchAppartament/{text}")]
        public IActionResult getAppartament(string text)
        {
            string searchLower = text.ToLower();
            List<AppartamentDTO> appartamentDTOs = new List<AppartamentDTO>();
            List<Place> houses = db.Places
                    .Where(h => h.Area.ToLower().Contains(searchLower) || h.Address.ToLower().Contains(searchLower))
                    .ToList();

            if (houses == null)
            {
                return StatusCode(500, "House data not found."); 
            }
            foreach(Place house in houses){
                Appartament appartament = db.Appartaments
                .Where(a => a.PlaceId == house.Id)
                .FirstOrDefault();
                if (appartament == null)
                {
                    return StatusCode(500, "Appartament data not found."); 
                }
                User owner = db.Users
                        .Where(o => o.Id == appartament.UserId)
                        .FirstOrDefault();
                if (owner == null)
                {
                    return StatusCode(500, "Owner data not found."); 
                }
                // User user = db.Users
                //         .Where(u => u.Id == owner.UserId)
                //         .FirstOrDefault();
                // if (user == null)
                // {
                //     return StatusCode(500, "User data not found.");
                // }
                List<Image> imageAppartament = db.Images
                        .Where(i => i.AppartamentId == appartament.Id)
                        .ToList();
                List<string> imageNames = db.Images
                        .Where(image => imageAppartament.Select(i => i.Id).Contains(image.Id))
                        .Select(image => image.ImagePath.Replace("uploads\\", ""))
                        .ToList();
                List<AppartamentAmenetie> appartamentAmeneties = db.AppartamentAmeneties
                        .Where(amenetie => amenetie.AppartamentId == appartament.Id).ToList();
                List<string> ameneties = db.Ameneties
                        .Where(a => appartamentAmeneties.Select(aa => aa.AmenetieId).Contains(a.Id))
                        .Select(a => a.Name).ToList();
                AppartamentDTO appartamentDTO = new AppartamentDTO(appartament, house, owner, imageNames, ameneties);
                appartamentDTOs.Add(appartamentDTO);
            }
            return Ok(appartamentDTOs);
        }

        [HttpGet]
        [Route("getAppartament/{appartamentId}")]
        public IActionResult getAppartament(int appartamentId)
        {
            Appartament appartament = db.Appartaments
                .Where(a => a.Id == appartamentId)
                .FirstOrDefault();
            if (appartament == null)
                {
                    return NotFound(); // Вернуть 404 Not Found, если квартира не найдена
                }

                // Загрузить данные о доме для данной квартиры
            Place house = db.Places
                    .Where(h => h.Id == appartament.PlaceId)
                    .FirstOrDefault();

            if (house == null)
            {
                return StatusCode(500, "House data not found."); // Вернуть 500 Internal Server Error, если данные о доме не найдены
            }
            User owner = db.Users
                    .Where(o => o.Id == appartament.UserId)
                    .FirstOrDefault();
            if (owner == null)
            {
                return StatusCode(500, "Owner data not found."); // Вернуть 500 Internal Server Error, если данные о доме не найдены
            }
            // User user = db.Users
            //         .Where(u => u.Id == owner.Id)
            //         .FirstOrDefault();
            // if (user == null)
            // {
            //     return StatusCode(500, "User data not found."); // Вернуть 500 Internal Server Error, если данные о доме не найдены
            // }
            List<Image> imageAppartament = db.Images
                    .Where(i => i.AppartamentId == appartament.Id)
                    .ToList();
            List<string> imageNames = db.Images
                    .Where(image => imageAppartament.Select(i => i.Id).Contains(image.Id))
                     .Select(image => image.ImagePath.Replace("uploads\\", ""))
                    .ToList();
            List<AppartamentAmenetie> appartamentAmeneties = db.AppartamentAmeneties
                        .Where(amenetie => amenetie.AppartamentId == appartament.Id).ToList();
            List<string> ameneties = db.Ameneties
                        .Where(a => appartamentAmeneties.Select(aa => aa.AmenetieId).Contains(a.Id))
                        .Select(a => a.Name).ToList();
            AppartamentDTO appartamentDTO = new AppartamentDTO(appartament, house, owner, imageNames, ameneties);
            return Ok(appartamentDTO);
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

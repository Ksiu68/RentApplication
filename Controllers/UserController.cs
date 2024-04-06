using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        //private Models.ApplicationContext db;

        //public UserController(Models.ApplicationContext context)
        //{
        //    db = context;
        //    if (!db.Users.Any())
        //    {
        //        db.Users.Add(new User { UserName = "Tom", Age = 26 });
        //        db.Users.Add(new User { Name = "Alice", Age = 31 });
        //        db.SaveChanges();
        //    }
        //}

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<User>>> Get()
        //{
        //    return await db.Users.ToListAsync();
        //}

        //// GET api/users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> Get(int id)
        //{
        //    User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
        //    if (user == null)
        //        return NotFound();
        //    return new ObjectResult(user);
        //}

        //// POST api/users
        //[HttpPost]
        //public async Task<ActionResult<User>> Post(User user)
        //{
        //    if (user == null)
        //    {
        //        return BadRequest();
        //    }

        //    db.Users.Add(user);
        //    await db.SaveChangesAsync();
        //    return Ok(user);
        //}

        //// PUT api/users/
        //[HttpPut]
        //public async Task<ActionResult<User>> Put(User user)
        //{
        //    if (user == null)
        //    {
        //        return BadRequest();
        //    }
        //    if (!db.Users.Any(x => x.Id == user.Id))
        //    {
        //        return NotFound();
        //    }

        //    db.Update(user);
        //    await db.SaveChangesAsync();
        //    return Ok(user);
        //}

        //// DELETE api/users/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<User>> Delete(int id)
        //{
        //    User user = db.Users.FirstOrDefault(x => x.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    db.Users.Remove(user);
        //    await db.SaveChangesAsync();
        //    return Ok(user);
        //}
    }
}

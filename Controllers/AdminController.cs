using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApplication.Models;

[Route("api/[controller]")]
[ApiController]
public class AdminController : Controller
{
    private ApplicationContext db;
    private readonly UserManager<User> userManager;
    public AdminController(ApplicationContext db, UserManager<User> userManager)
    {
        this.db = db;
        this.userManager = userManager;
    }

    // GET: api/User
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await db.Users.ToListAsync();
    }

    // GET: api/User/5
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        var user = await db.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // PUT: api/User/5
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(string id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }
        var user1 = await userManager.FindByIdAsync(id);
        try
        {
            var result = await userManager.UpdateAsync(user1);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete user.");
        }

        return NoContent();
    }

    private bool UserExists(string id)
    {
        return db.Users.Any(e => e.Id == id);
    }

     // GET: api/Review
    [HttpGet]
    [Route("Review")]
    public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
    {
        return await db.Reviews.ToListAsync();
    }

    // GET: api/Review/5
    [HttpGet("Review/{id}")]
    public async Task<ActionResult<Review>> GetReview(int id)
    {
        var review = await db.Reviews.FindAsync(id);

        if (review == null)
        {
            return NotFound();
        }

        return review;
    }

    // PUT: api/Review/5
    [HttpPut("Review/{id}")]
    public async Task<IActionResult> PutReview(int id, Review review)
    {
        if (id != review.Id)
        {
            return BadRequest();
        }

        db.Entry(review).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReviewExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Review/5
    [HttpDelete("Review/{id}")]
    public IActionResult DeleteReview(int id)
    {
        var review = db.Reviews.Find(id);
        if (review == null)
        {
            return NotFound();
        }

        db.Reviews.Remove(review);
        db.SaveChanges();

        return NoContent();
    }

    private bool ReviewExists(int id)
    {
        return db.Reviews.Any(e => e.Id == id);
    }


    // GET: api/Appartament
    [HttpGet]
    [Route("Appartament")]
    public async Task<ActionResult<IEnumerable<Appartament>>> GetAppartaments()
    {
        return await db.Appartaments.ToListAsync();
    }

    // GET: api/Appartament/5
    [HttpGet("Appartament/{id}")]
    public async Task<ActionResult<Appartament>> GetAppartament(int id)
    {
        var appartament = await db.Appartaments.FindAsync(id);

        if (appartament == null)
        {
            return NotFound();
        }

        return appartament;
    }

    // PUT: api/Appartament/5
    [HttpPut("Appartament/{id}")]
    public async Task<IActionResult> PutAppartament(int id, Appartament appartament)
    {
        if (id != appartament.Id)
        {
            return BadRequest();
        }

        db.Entry(appartament).State = EntityState.Modified;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AppartamentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

        // DELETE: api/Appartament/5
    [HttpDelete("Appartament/{id}")]
    public IActionResult DeleteAppartament(int id)
    {
        var appartament = db.Appartaments.Find(id);
        if (appartament == null)
        {
            return NotFound();
        }

        db.Appartaments.Remove(appartament);
        db.SaveChanges();

        return NoContent();
    }

    private bool AppartamentExists(int id)
    {
        return db.Appartaments.Any(e => e.Id == id);
    }
}
using Microsoft.AspNetCore.Mvc;
using SafeCampus.Models;
using SafeCampus.Services;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;
using Amazon.Auth.AccessControlPolicy;
using Safe_Campus.Services;
using Safe_Campus.Models;

[ApiController]
[Route("api/[controller]")]
public class LaptopController : ControllerBase
{
    private readonly LaptopService _laptopService;
    private readonly IUserService _userService;

    public LaptopController(LaptopService laptopService, IUserService userService)
    {
        _laptopService = laptopService;
        _userService = userService;
    }

    // GET: api/laptop
    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<List<Laptop>>> GetAsync()
    {
        var laptops = await _laptopService.GetAsync();

        return Ok(laptops);
    }

    // GET: api/laptop/{id}
    [HttpGet("{id:length(24)}", Name = "GetLaptop")]
    [Authorize(Roles = "Admin, Guard")]
    
    public async Task<ActionResult<LaptopResponseDto>> GetAsync(string id)
    {
        var laptop =  _laptopService.GetAsync(id);


        if (laptop == null)
        {
            return NotFound();
        }
        var ownerId = laptop.Result.OwnerId;
        var student = _userService.GetByName(ownerId);
        var result = new { Laptop = laptop.Result ,User=student };
        return Ok(result);
    }

    // POST: api/laptop
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<Laptop>> CreateAsync([FromBody] Laptop newLaptop)
    {
        try
        {
            
            
            var laptopOwner = _laptopService.GetByOwner(newLaptop.OwnerId);
            var serial = _laptopService.GetBySerial(newLaptop.SerialNumber);
            if (laptopOwner is not null)
            {
                return BadRequest("This user have already registered!");
            }
            if (serial is not null)
            {
                return BadRequest("This PC have already registered!");
            }
           
            await _laptopService.CreateAsync(newLaptop);
            return CreatedAtRoute("GetLaptop", new { id = newLaptop.Id }, newLaptop.Id);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error creating laptop: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }


    // PUT: api/laptop/{id}
    [HttpPut("{id:length(24)}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Laptop updatedLaptop)
    {
        var existingLaptop = await _laptopService.GetAsync(id);

        if (existingLaptop == null)
        {
            return NotFound();
        }

        await _laptopService.UpdateAsync(id, updatedLaptop);
        return NoContent();
    }

    // DELETE: api/laptop/{id}
    [HttpDelete("{id:length(24)}"), Authorize(Roles ="Admin")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var laptop = await _laptopService.GetAsync(id);

        if (laptop == null)
        {
            return NotFound();
        }

        await _laptopService.RemoveAsync(id);
        return NoContent();
    }
}

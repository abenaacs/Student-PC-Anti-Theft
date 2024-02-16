using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Safe_Campus.Models;
using Safe_Campus.Services;
using SafeCampus.Models;
using SafeCampus.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Safe_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _environment;

        public UserController(IUserService userService, IWebHostEnvironment environment)
        {
            _userService = userService;
            _environment = environment;
        }
        // GET: api/<UserController>
        [HttpGet("Student"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<User>>> GetStudent()
        {
            return await _userService.GetStudent();
        }
        // GET: api/<UserController>
        [HttpGet("Guard"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<User>>> GetGuard()
        {
            return await _userService.GetGuard();
        }

        // POST api/<UserController>
        [HttpPost, Authorize(Roles = "Admin")]

        public async Task<ActionResult<User>> RegisterUser(RegisterDto request)
        {
            User user = new User();
            if (_userService.CheckUser(request.UserName))
            {
                return BadRequest("User Already exists !");
            }
            else if (request.Role == "Admin")
            {
                return BadRequest("Invalid Input!");
            }
            //encrypting the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            //storing the data on the database
            user.PasswordHash = passwordHash;
            user.UserName = request.UserName;
            user.Role = request.Role;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.ProfilePicture = request.ProfilePicture;
            user.ContactNumber = request.ContactNumber;
            user.Sex = request.Sex;

            await _userService.Create(user);
            return CreatedAtAction(nameof(RegisterUser), user);
        }


        // GET api/<UserController>/5

        [HttpGet("{id:length(24)}", Name = "GetUser"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT api/<UserController>
        [HttpPut("{id:length(24)}", Name = "UpdateUser"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> Put(string Id, User user)
        {
            var existingUser = await _userService.Get(Id); 
            if (existingUser == null) {
                return NotFound($"the user with { Id } cannot be found");
            }
            await  _userService.Update(Id,user);
            return NoContent();
        }




        // DELETE api/<UserController>
        [HttpDelete("{id:length(24)}", Name = "DeleteUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var existingUser = await _userService.Get(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            await _userService.RemoveUser(id);

            return NoContent();
        }



        
       
        [HttpPut("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile formFile, string userName)
        {
            APIResponse response = new APIResponse();
            var user = _userService.GetByName(userName);
            
            try
            {
                if (user == null)
                {
                    return NotFound();
                }

                string Filepath = GetFilepath(userName);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }

                string imagepath = Filepath + "\\" + userName + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await formFile.CopyToAsync(stream);
                    response.ResponseCode = 200;
                    response.Result = "pass";
                    await _userService.UpdateImage(user.Id, Filepath);
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            
            return Ok(response);
        }
        [NonAction]
        private string GetFilepath(string userName)
        {
            return _environment.WebRootPath + "\\UploadImage\\" + userName;
        }
    }
}

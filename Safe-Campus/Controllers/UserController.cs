using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Safe_Campus.Models;
using Safe_Campus.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Safe_Campus.Controllers
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
           
        }
        // GET: api/<UserController>
        [HttpGet("Get-All-data"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<User>>> GetAll(string role)
        {
            return await _userService.GetAll(role);
        }
    
        // GET api/<UserController>/5

        [HttpGet("Get-User"), Authorize(Roles ="Admin,Student, Guard")]
        public async Task<ActionResult<User>> GetByUserName(string userName)
        {
            var existingUser = _userService.GetByName(userName);
            return await _userService.GetById(existingUser.Id);
        }

        // PUT api/<UserController>
        [HttpPut("Update-Student"), Authorize(Roles ="Admin")]
        public ActionResult Put(string userName, User user)
        {
            var existingUser = _userService.GetByName(userName); 
            if (existingUser == null) {
                return NotFound($"the user with { userName } cannot be found");
            }
            _userService.Update(existingUser.Id,user);
            return NoContent();
        }

        // DELETE api/<UserController>
        [HttpDelete("DeleteStudent"), Authorize(Roles ="Admin")]
        public ActionResult DeleteStudent(string userName)
        { User user = _userService.GetByName(userName);

            _userService.Remove(user.Id); 
            return Ok($"The user with {user.Id} is deleted successfully");
        }
    }
}

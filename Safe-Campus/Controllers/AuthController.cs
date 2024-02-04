﻿using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Safe_Campus.Models;
using Safe_Campus.Services;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Safe_Campus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        [HttpGet, Authorize]
        public ActionResult GetMyName() { 
            return Ok(_userService.GetMyName());
        }

        // POST api/<UserController>
        [HttpPost("Student-Register"), Authorize(Roles ="Admin")]

        public async Task<ActionResult<User>> RegisterStudent(Student request)
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
            user.AdmissionType = request.AdmissionType;
            user.StudyLevel = request.StudyLevel;
            user.Department = request.Department;
            user.StartDate = request.StartDate;
            user.EndDate = request.GraduateDate; 

            await  _userService.Create(user);
            return CreatedAtAction(nameof(RegisterStudent), user);
        }

        //Post api/<UserController>
        [HttpPost("Guard-Register"), Authorize(Roles ="Admin")]

        public async Task<ActionResult<User>> RegisterGuard(GuardDto request)
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
            //storing the data on database
            user.PasswordHash = passwordHash;
            user.UserName = request.UserName;
            user.Role = request.Role;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.ProfilePicture = request.ProfilePicture;
            user.ContactNumber = request.ContactNumber;
            user.Sex = request.Sex;

            await _userService.Create(user);
            return CreatedAtAction(nameof(RegisterStudent), user);
        }

        // POST api/<UserController>
        [HttpPost("Login")]
        public ActionResult<string> Login(UserDto request)
        {
            //accessing the user data from database based on the userNAme
            var user = _userService.GetByName(request.UserName);

            if (user==null)
            {
                return BadRequest("The user doesn't exist!");

            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Incorrect password !");
            }
            //creating a token
            string token = CreateToken(request.UserName, user.Role);
            var refreshToken = GenerateRefreshToken();
            setRefreshToken(refreshToken, user);

            return Ok(token);

        }

        // POST api/<UserController>
        [HttpPost("Refresh-Token")]
        public async Task<ActionResult<string>> RefreshToken()
        {

            var refreshToken = Request.Cookies["refreshToken"];
            var user = _userService.GetByToken(refreshToken);
            if (user == null)
            {
                return Unauthorized("Invalid refresh token");
            }
            else if (user.RefreshTokenExpires < DateTime.Now)
            {
                return Unauthorized("token expired");
            }
            string token = CreateToken(user.UserName, user.Role);
            var newRefreshToken = GenerateRefreshToken();
            setRefreshToken(newRefreshToken, user);
            return Ok(token);
        }

        //Generating a new Refreshing Token
         private RefreshToken GenerateRefreshToken()
         {
             var refreshToken = new RefreshToken
             {
                 Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                 Expires = DateTime.Now.AddDays(7),
                 Created = DateTime.Now
             };
             return refreshToken;
         }

        //setting Refreshed Token both on Cookies and database
         private void setRefreshToken(RefreshToken newRefreshToken, User user)
         {  
             var cookieOptions = new CookieOptions
             {
                 HttpOnly = true,
                 Expires = newRefreshToken.Expires,
             };
             Response.Cookies.Append("refreshToken",newRefreshToken.Token, cookieOptions);
            user.RefreshToken = newRefreshToken.Token;
            user.RefreshTokenExpires = newRefreshToken.Expires;
            user.RefreshTokenCreated = newRefreshToken.Created;
            _userService.UpdateRefreshToken(user);
         }
         
        //Creatin A Token
        private string CreateToken(string userName, string userRole)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userName)
                };
            if (userRole == "Admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            if (userRole == "Student")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Student"));
            }
            if (userRole == "Guard")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Guard"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(30),
                signingCredentials: creds

                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }



    }
}

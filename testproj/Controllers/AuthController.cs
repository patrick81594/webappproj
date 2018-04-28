﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testproj.Data;
using testproj.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace testproj.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]

    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;


        public AuthController(IAuthRepository repo, IConfiguration configuration)
        {
            Configuration = configuration;
            _repo = repo;
        }


        public IConfiguration Configuration { get; }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Make user name lower case
            user.UserName = user.UserName.ToLower();

            // If duplicate user name and return bad request here
            // Need method in AuthRepo to test for this
            
            if (_repo.ValidateUserName(user.UserName))
            {
                ModelState.AddModelError("UserName", "User name already exists");
                return BadRequest(ModelState);
            }

            var newUser = await _repo.Register(user.UserName, user.Password);
            // Temporary return result for testing
            return StatusCode(201, new { ID = newUser.ID, UserName = newUser.UserName });
        }
        
        [HttpPost("login")]
     
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            var storedUser = await _repo.Login(user.UserName, user.Password);
            if (storedUser == null)
            {
                return Unauthorized();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("TokenSettings:JWTKey").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, storedUser.ID.ToString()),
 new Claim(ClaimTypes.Name, storedUser.UserName)
 }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
 SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            // Temporary return value for testing
            return Ok(new { ID = storedUser.ID, UserName = storedUser.UserName, tokenString = tokenString });
        }
    }
}
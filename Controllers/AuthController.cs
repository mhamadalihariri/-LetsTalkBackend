using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Let_sTalk.Data;
using Let_sTalk.DTOS;
using Let_sTalk.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Let_sTalk.Models;
using Let_sTalk.Data.IRepos;

namespace MobileAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IUserPreferenceReporsitory userPreferenceRepository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repository, IUserPreferenceReporsitory _userPrefRepo, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
            userPreferenceRepository = _userPrefRepo;
        }

        [HttpPost("register")]
        public IActionResult RegisterAsync(RegisterDTO dto)
        {
            var exsistingUser = _repository.getByEmail(dto.Email);
            if (exsistingUser != null)
            {
                return BadRequest(new
                {
                    statusCode = 422,
                    message = "Email already in use"
                });
            }

            var user = new User
            {
                FirstName = dto.Firstname,
                LastName = dto.Lastname,
                Email = dto.Email,
                DOB = dto.Dob,
                Image = dto.Image,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Gender = dto.Gender,
                PhoneNumber = dto.Phone
            };
            var u = _repository.create(user);
            Preference selectedPreference = dto.Preference;
            var newuserPref = userPreferenceRepository.create(new UserPreference
            {
                PreferenceId = selectedPreference.Id,
                UserId = u.Id
            });
                u.UserPreferences = new List<UserPreference>();
                u.UserPreferences.Add(newuserPref);
             
            //var updatedUser = _repository.update(u);
            //UserPreference userPreference = new UserPreference();
            //userPreference.PreferenceId = dto.Preference.IdPreference;
            //userPreference.UserId = u.IdUser;
            //var savedUserPref = userPreferenceRepository.create(userPreference);
            //u.UserPreferences.Add(savedUserPref);
            return Ok(new
            {
                statusCode = 200,
                message = "Success",
            });
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var user = _repository.getByEmail(dto.Email);
            if (user == null) return BadRequest(new
            {
                statusCode = 401,
                message = "Invalid Credentials"
            });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return Unauthorized(new
                {
                    statusCode = 401,
                    message = "Invalid Credentials"
                });
            }
            var jwt = _jwtService.Generate(user.Id);
            Response.Cookies.Append("token", jwt, new CookieOptions
            {
                HttpOnly = true
            });
            return Ok(new
            {
                statusCode = 200,
                message = "Success",
                token = jwt
            });
        }

        [HttpGet("user")]
        public IActionResult User(string jwtString)
        {

            try
            {
                //var jwtString = Request.Cookies["token"];

                var token = _jwtService.Verify(jwtString);
                int userId = int.Parse(token.Issuer);
                var user = _repository.getById(userId);
                Console.WriteLine("Retrieved user:" + user.Email);
                return Ok(user);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }

        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Ok(new
            {
                statusCode = 200,
                message = "Success"
            });
        }

    }
}

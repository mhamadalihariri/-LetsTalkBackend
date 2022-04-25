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
using LetsTalkBackend.DTOS;

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
            try
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
                    PhoneNumber = dto.Phone,
                    FirebaseId = dto.FirebaseId
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
                return Ok(new
                {
                    statusCode = 200,
                    message = "Success",
                    //user = u
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new
                {
                    statusCode = 400,
                    message = "An Error has occured while trying to register user",
                });
            }
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            User user = _repository.getByEmail(dto.Email);
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

            UserDTO userDTO = new UserDTO();
            userDTO.Email = user.Email;
            userDTO.Id = user.Id;
            userDTO.Firstname = user.FirstName;
            userDTO.Lastname = user.LastName;
            userDTO.PhoneNumber = user.PhoneNumber;
            userDTO.DOB = user.DOB;
            userDTO.Gender = user.Gender;
            userDTO.Matches = user.Matches;
            userDTO.Location = user.Location;
            userDTO.UserPreferences = user.UserPreferences;
            userDTO.FirebaseId = user.FirebaseId;

            var jwt = _jwtService.Generate(user.Id);
            Response.Cookies.Append("token", jwt, new CookieOptions
            {
                HttpOnly = true
            });
            return Ok(new
            {
                statusCode = 200,
                message = "Success",
                token = jwt,
                user = userDTO,
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
                //Console.WriteLine("Retrieved user:" + user.Email);
                UserDTO userDTO = new UserDTO();
                userDTO.Email = user.Email;
                userDTO.Id = user.Id;
                userDTO.Firstname = user.FirstName;
                userDTO.Lastname = user.LastName;
                userDTO.PhoneNumber = user.PhoneNumber;
                userDTO.DOB = user.DOB;
                userDTO.Gender = user.Gender;
                userDTO.Matches = user.Matches;
                userDTO.Location = user.Location;
                userDTO.UserPreferences = user.UserPreferences;
                userDTO.FirebaseId = user.FirebaseId;

                return Ok(userDTO);
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

        [HttpGet("checkUserExists/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            User user = _repository.getByEmail(email);
            if(user == null)
            {
                return NotFound();
            }
            else
            {
                UserDTO userDTO = new UserDTO();
                userDTO.Email = user.Email;
                userDTO.Id = user.Id;
                userDTO.Firstname = user.FirstName;
                userDTO.Lastname = user.LastName;
                userDTO.PhoneNumber = user.PhoneNumber;
                userDTO.DOB = user.DOB;
                userDTO.Gender = user.Gender;
                userDTO.Matches = user.Matches;
                userDTO.Location = user.Location;
                userDTO.UserPreferences = user.UserPreferences;
                userDTO.FirebaseId = user.FirebaseId;

                return Ok(userDTO);
            }
        }

    }
}

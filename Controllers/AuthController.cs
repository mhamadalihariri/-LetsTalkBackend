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
using LetsTalkBackend.Helpers;
using LetsTalkBackend.Models;

namespace MobileAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IUserPreferenceReporsitory userPreferenceRepository;
        private readonly JwtService _jwtService;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IMailService mailService;

        public AuthController(IUserRepository repository, IUserPreferenceReporsitory _userPrefRepo, IPreferenceRepository preferenceRepository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
            userPreferenceRepository = _userPrefRepo;
            _preferenceRepository = preferenceRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDTO dto)
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
                Location userLocation = new Location();
                userLocation.Longitude = dto.Location.Longitude;
                userLocation.Latitude = dto.Location.Latitude;
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
                    FirebaseId = dto.FirebaseId,
                    Location = userLocation
                };
                var u = _repository.createOrUpdate(user);
                List<Preference> selectedPreferences = dto.Preferences;
                if (selectedPreferences != null)
                {
                    foreach (Preference selectedPreference in selectedPreferences)
                    {
                        var newuserPref = userPreferenceRepository.create(new UserPreference
                        {
                            PreferenceId = selectedPreference.Id,
                            UserId = u.Id
                        });
                        u.UserPreferences = new List<UserPreference>();
                        u.UserPreferences.Add(newuserPref);
                    }
                }
                WelcomeRequest welcomeRequest = new WelcomeRequest();
                welcomeRequest.ToEmail = dto.Email;
                welcomeRequest.UserName = dto.Firstname + " " + dto.Lastname;
                await mailService.SendWelcomeEmailAsync(welcomeRequest);
                return Ok(new
                {
                    statusCode = 200,
                    message = "Success",
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
        //[ValidateAntiForgeryToken]
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

            List<Preference> preferences = new List<Preference>();
            List<UserPreference> userPrefs = user.UserPreferences;
            foreach (UserPreference pref in userPrefs)
            {
                preferences.Add(_preferenceRepository.getById(pref.PreferenceId));
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
            userDTO.Preferences = preferences;
            userDTO.FirebaseId = user.FirebaseId;
            userDTO.Location = user.Location;

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
                List<Preference> preferences = new List<Preference>();
                List<UserPreference> userPrefs = user.UserPreferences;
                foreach (UserPreference pref in userPrefs)
                {
                    preferences.Add(_preferenceRepository.getById(pref.PreferenceId));
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
                userDTO.Preferences = preferences;
                userDTO.FirebaseId = user.FirebaseId;
                userDTO.Location = user.Location;


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
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                List<Preference> preferences = new List<Preference>();
                List<UserPreference> userPrefs = user.UserPreferences;
                foreach (UserPreference pref in userPrefs)
                {
                    preferences.Add(_preferenceRepository.getById(pref.PreferenceId));
                }
                    UserDTO userDTO = new UserDTO();
                    //userDTO.Email = user.Email;
                    userDTO.Id = user.Id;
                   // userDTO.Firstname = user.FirstName;
                   //userDTO.Lastname = user.LastName;
                   // userDTO.PhoneNumber = user.PhoneNumber;
                   // userDTO.DOB = user.DOB;
                   // userDTO.Gender = user.Gender;
                    //userDTO.Matches = user.Matches;
                    //userDTO.Location = user.Location;
                    //userDTO.Preferences = preferences;
                   // userDTO.FirebaseId = user.FirebaseId;
                    return Ok(userDTO);

            }
            
        }

        [HttpPost("update")]
        //[ValidateAntiForgeryToken]
        public IActionResult UpdateUser(RegisterDTO registerDTO)
        {
            try
            {
                Location userLocation = new Location();
                userLocation.Longitude = registerDTO.Location.Longitude;
                userLocation.Latitude = registerDTO.Location.Latitude;
                var user = new User
                {
                    FirstName = registerDTO.Firstname,
                    LastName = registerDTO.Lastname,
                    Email = registerDTO.Email,
                    DOB = registerDTO.Dob,
                    Image = registerDTO.Image,
                    Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                    Gender = registerDTO.Gender,
                    PhoneNumber = registerDTO.Phone,
                    FirebaseId = registerDTO.FirebaseId,
                    Location = userLocation


                };
                var updatedUser = _repository.createOrUpdate(user);
                List<Preference> selectedPreferences = registerDTO.Preferences;
                if (selectedPreferences != null)
                {
                    foreach (Preference selectedPreference in selectedPreferences)
                    {
                        if(updatedUser.UserPreferences.Any(up => up.PreferenceId == selectedPreference.Id) == false)
                        {
                            var newuserPref = userPreferenceRepository.create(new UserPreference
                            {
                                PreferenceId = selectedPreference.Id,
                                UserId = updatedUser.Id
                            });
                            updatedUser.UserPreferences = new List<UserPreference>();
                            updatedUser.UserPreferences.Add(newuserPref);
                        }
                    }
                }

                return Ok(new
                {
                    statusCode = 200,
                    message = "Success",
                });
            }catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("changePassword/{userId}")]
        public IActionResult ChangePassword(int userId, string newPassword)
        {
            try
            {
                _repository.ChangePassword(userId, newPassword);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}

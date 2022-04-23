using Let_sTalk.Data.IRepos;
using Let_sTalk.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Let_sTalk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPreferenceController : ControllerBase
    {
        private readonly IUserPreferenceReporsitory _repository;

        public UserPreferenceController(IUserPreferenceReporsitory userPreferenceReporsitory)
        {
            _repository = userPreferenceReporsitory;
        }

        [HttpGet("all")]
        public IActionResult UserPreferences()
        {
            try
            {
                var userprefs = _repository.GetAllUserPreferences();
                return Ok(userprefs);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public IActionResult UserPreference(int id)
        {
            try
            {
                var userpref = _repository.GetUserPreferenceById(id);
                return Ok(userpref);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpGet("user/{id}")]
        public IActionResult UserPreferencesByUserId(int id)
        {
            try
            {
                var userprefs = _repository.GetUserPreferencesByUserId(id);
                return Ok(userprefs);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpGet("preference/{id}")]
        public IActionResult UserPreferencesByPreferenceId(int id)
        {
            try
            {
                var userprefs = _repository.GetUserPreferencesByPreferenceId(id);
                return Ok(userprefs);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult CreateUserPreference(UserPreference userPreference)
        {
            try
            {
                var userpref = _repository.create(userPreference);
                return Ok(userpref);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }

        [HttpPut]
        public IActionResult UpdateUserPreference(UserPreference userPreference)
        {
            try
            {
                var userpref = _repository.update(userPreference);
                return Ok(userpref);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }

        [HttpDelete]
        public IActionResult DeleteUserPreference(int id)
        {
            try
            {
            var userpref = _repository.GetUserPreferenceById(id);
            _repository.delete(userpref);
                return Ok(
                    new {
                    statusCode = 200,
                    message = "Success",
                });
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}

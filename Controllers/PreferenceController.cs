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
using Microsoft.AspNetCore.Cors;

namespace Let_sTalk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenceController : ControllerBase
    {
        private readonly IPreferenceRepository _repository;
        private readonly JwtService _jwtService;

        public PreferenceController(IPreferenceRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("add")]
        public IActionResult AddAsync(Preference pref)
        {
            var existingPref = _repository.getByName(pref.CuisineName);
            if (existingPref != null)
            {
                return BadRequest(new
                {
                    statusCode = 422,
                    message = "Preference already exists"
                });
            }
            var p = _repository.create(pref);
            return Ok(new
            {
                statusCode = 200,
                message = "Success",
            });
        }

        [HttpGet("{id}")]
        public IActionResult Preference(int id)
        {

            try
            {
                var pref = _repository.getById(id);
                return Ok(pref);
            }
            catch (Exception e)
            {
                return NotFound();
            }

        }

        [HttpGet("all")]
        public IActionResult Preferences()
        {
            try
            {
                var prefs = _repository.getAll();
                return Ok(prefs);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpDelete("preference")]
        public IActionResult Delete(int id)
        {
            try
            {
                var pref = _repository.getById(id);
                _repository.deleteById(pref);
                return Ok(new
                {
                    statusCode = 200,
                    message = "Success",
                });
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}

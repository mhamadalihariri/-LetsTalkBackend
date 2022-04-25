using Microsoft.AspNetCore.Mvc;
using Let_sTalk.Data.IRepos;
using Let_sTalk.DTOS;
using Let_sTalk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Let_sTalk.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchRepository _repository;

        public MatchController(IMatchRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("matching")]
        public IActionResult MatchAsync(MatchDTO dto)
        {
            try
            {
                var exsistingUser = _repository.getMatch(dto.User1, dto.User2);
                //Console.WriteLine(exsistingUser);
                var match = new Match
                {
                    User1 = dto.User1,
                    User2 = dto.User2
                };
                if (exsistingUser == null)
                {
                    match.IsMatched = 0;
                    var m = _repository.create(match);
                    return Ok(new
                    {
                        statusCode = 200,
                        message = "Match 1",
                    });
                }
                else
                {
                    match.IsMatched = 1;
                    _repository.updateIfMatch(dto.User1, dto.User2);
                    var createdMatch = _repository.create(match);
                    return Ok(new
                    {
                        statusCode = 200,
                        message = "Match 2",
                    });
                }

                
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    statusCode = 404,
                    message = "Internal Server Error",
                    Exception = e
                });
            }
            
        }

        [HttpGet("getMatchesByUserId/{id}")]
        public IActionResult GetMatchesByUserId(int id)
        {
            try
            {
                var matchingUsers = _repository.getMatchesByUserId(id);
                Console.WriteLine(matchingUsers);

                return Ok(new
                {
                    statusCode = 200,
                    message = "",
                    firebaseIdForMatchingUsers = matchingUsers,
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    statusCode = 404,
                    message = "Internal Server Error",
                    Exception = e
                });
            }

        }

    }
}

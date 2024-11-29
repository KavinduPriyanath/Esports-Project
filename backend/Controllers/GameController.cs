using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.Game;
using backend.Dtos.User;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("backend/game")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IUserRepository _userRepo;
        private readonly IGameRepository _gameRepo;

        public GameController(ApplicationDBContext context, IUserRepository userRepo, IGameRepository gameRepo)
        {
            _userRepo = userRepo;
            _context = context;
            _gameRepo = gameRepo;
        }

        private string GetCurrentUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email); // Assuming the ID is stored in this claim
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var allGames = await _gameRepo.GetAllAsync();
            var allGamesDto = allGames.Select(g => g.ToGameDto());
            return Ok(allGamesDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGameById([FromRoute] int id)
        {
            var gameDetails = await _gameRepo.GetByIdAsync(id);
            if (gameDetails == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(gameDetails);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDto createGameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var gameModel = createGameDto.ToGameFromGameCreateDto();
            await _gameRepo.CreateAsync(gameModel);
            return CreatedAtAction(nameof(GetGameById), new { id = gameModel.GameId }, gameModel.ToGameDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateGame([FromRoute] int id, [FromBody] UpdateGameDto updateGameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserEmail = GetCurrentUserEmail();
            if (string.IsNullOrEmpty(currUserEmail))
            {
                return Unauthorized($"User not found {currUserEmail}");
            }
            var currUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == currUserEmail);
            // Only Site Admins Can change game details? Normal users are not allowed. Define an attribute for usermodel to state the admin privilleges or some other technique.
            var gameDetails = await _gameRepo.GetByIdAsync(id);
            // if (currUser.UserId == organizationDetails.Owner || currUser.UserId == organizationDetails.Admin1 || currUser.UserId == organizationDetails.Admin2 || currUser.UserId == organizationDetails.Admin3)
            // {
            var gameModel = await _gameRepo.UpdateAsync(id, updateGameDto);
            if (gameModel == null)
            {
                return NotFound();
            }
            return Ok(gameModel.ToGameDto());
            // }
            // else
            // {
            //     return Unauthorized("User is not authorized to update organization details");
            // }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteGame([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserEmail = GetCurrentUserEmail();
            if (string.IsNullOrEmpty(currUserEmail))
            {
                return Unauthorized($"User not found {currUserEmail}");
            }
            var currUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == currUserEmail);
            // Only Site Admins Can change game details? Normal users are not allowed. Define an attribute for usermodel to state the admin privilleges or some other technique.
            var gameDetails = await _gameRepo.GetByIdAsync(id);
            // if (currUser.UserId == organizationDetails.Owner || currUser.UserId == organizationDetails.Admin1 || currUser.UserId == organizationDetails.Admin2 || currUser.UserId == organizationDetails.Admin3)
            // {
            var gameModel = await _gameRepo.DeleteAsync(id);
            if (gameModel == null)
            {
                return NotFound();
            }
            return Ok(gameModel.ToGameDto());
            // }
            // else
            // {
            //     return Unauthorized("User is not authorized to update organization details");
            // }
        }
    }

}
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
using backend.Helpers;

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
        private readonly UserHelper _userHelper;

        public GameController(ApplicationDBContext context, IUserRepository userRepo, IGameRepository gameRepo, UserHelper userHelper)
        {
            _userRepo = userRepo;
            _context = context;
            _gameRepo = gameRepo;
            _userHelper = userHelper;
        }

        // Get all games
        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var allGames = await _gameRepo.GetAllAsync();
            var allGamesDto = allGames.Select(g => g.ToGameDto());
            return Ok(allGamesDto);
        }

        // Get details of the selected game
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

        // Create a new game
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

        // Update a game
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateGame([FromRoute] int id, [FromBody] UpdateGameDto updateGameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            // Only Sys Admins Can change game details? Normal users are not allowed. Define an attribute for usermodel to state the admin privilleges or some other technique.
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

        // Delete a game - Only game status is changed. Notv permenently deleted.
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteGame([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            // Only Site Admins Can change game details? Normal users are not allowed. Define an attribute for usermodel to state the admin privilleges or some other technique.
            var gameDetails = await _gameRepo.GetByIdAsync(id);
            var gameModel = await _gameRepo.DeleteAsync(id);
            if (gameModel == null)
            {
                return NotFound();
            }
            return Ok(gameModel.ToGameDto());
        }
    }

}
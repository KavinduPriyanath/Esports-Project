using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.User;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("backend/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IUserRepository _userRepo;
        public UserController(ApplicationDBContext context, IUserRepository userRepo)
        {
            _userRepo = userRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allUsers = await _userRepo.GetAllAsync();
            var allUsersDto = allUsers.Select(u => u.ToUserDto());
            return Ok(allUsersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user.ToUserDto());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var userModel = userDto.ToUserFromUserCreateDto();
            await _userRepo.CreateAsync(userModel);
            return CreatedAtAction(nameof(GetUserById), new { id = userModel.UserId }, userModel.ToUserDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto userDto)
        {
            var userModel = await _userRepo.UpdateAsync(id, userDto);
            if (userModel == null)
            {
                return NotFound();
            }
            return Ok(userModel.ToUserDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var userModel = await _userRepo.DeleteAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
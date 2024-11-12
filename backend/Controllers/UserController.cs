using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.User;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("backend/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allUsers = _context.Users.ToList().Select(u => u.ToUserDto());
            return Ok(allUsers);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById([FromRoute] int id)
        {
            var user = _context.Users.Find(id);
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
        public IActionResult CreateUser([FromBody] CreateUserDto userDto)
        {
            var userModel = userDto.ToUserFromUserCreateDto();
            _context.Users.Add(userModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUserById), new { id = userModel.UserId }, userModel.ToUserDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto userDto)
        {
            var userModel = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (userModel == null)
            {
                return NotFound();
            }
            userModel.FirstName = userDto.FirstName;
            userModel.LastName = userDto.LastName;
            userModel.UserName = userDto.UserName;
            userModel.UserEmail = userDto.UserEmail;
            userModel.Nic = userDto.Nic;
            userModel.Dob = userDto.Dob;
            userModel.ContactNo = userDto.ContactNo;
            userModel.Address = userDto.Address;
            userModel.ProfileImage = userDto.ProfileImage;
            userModel.WorkingStatus = userDto.WorkingStatus;
            userModel.UserAccountStatus = userDto.UserAccountStatus;
            _context.SaveChanges();
            return Ok(userModel.ToUserDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            var userModel = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (userModel == null)
            {
                return NotFound();
            }
            _context.Users.Remove(userModel);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
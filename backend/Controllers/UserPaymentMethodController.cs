using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.PaymentMethod;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("backend/paymentmethod")]
    [ApiController]
    [Authorize]
    public class UserPaymentMethodController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IPaymentMethodRepository _paymentMethodRepo;
        private readonly IUserRepository _userRepo;
        public UserPaymentMethodController(ApplicationDBContext context, IPaymentMethodRepository paymentMethodRepo, IUserRepository userRepo)
        {
            _paymentMethodRepo = paymentMethodRepo;
            _context = context;
            _userRepo = userRepo;
        }

        private string GetCurrentUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email); // Assuming the ID is stored in this claim
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaymentMethods()
        {
            var allPaymentMethods = await _paymentMethodRepo.GetAllAsync();
            var allPaymentMethodsDto = allPaymentMethods.Select(pm => pm.ToPaymentMethodDto());
            return Ok(allPaymentMethodsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPaymentMethodById([FromRoute] int id)
        {
            var paymentMethodDetails = await _paymentMethodRepo.GetByIdAsync(id);
            if (paymentMethodDetails == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(paymentMethodDetails.ToPaymentMethodDto());
            }
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetPaymentMethodsByUserId([FromRoute] int userId)
        {
            var paymentMethods = await _paymentMethodRepo.GetAllByIdAsync(userId);
            if (!paymentMethods.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(paymentMethods.Select(pm => pm.ToPaymentMethodDto()));
            }
        }

        // [HttpPost("{uid:int}")]
        // public async Task<IActionResult> CreatePaymentMethod([FromRoute] int uid, [FromBody] CreatePaymentMethodDto paymentMethodDto)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }
        //     if (!await _userRepo.IsUserExist(uid))
        //     {
        //         return BadRequest("User does not exists");
        //     }
        //     var paymentMethodModel = paymentMethodDto.ToUserPaymentMethodFromCreateDto(uid);
        //     await _paymentMethodRepo.CreateAsync(paymentMethodModel);
        //     return CreatedAtAction(nameof(GetPaymentMethodById), new { id = paymentMethodModel.UserPaymentMethodId }, paymentMethodModel.ToPaymentMethodDto());
        // }
        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] CreatePaymentMethodDto paymentMethodDto)
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
            if (currUser == null)
            {
                return NotFound($"User details not found {currUserEmail}");
            }
            var paymentMethodModel = paymentMethodDto.ToUserPaymentMethodFromCreateDto(currUser.UserId);
            await _paymentMethodRepo.CreateAsync(paymentMethodModel);
            return CreatedAtAction(nameof(GetPaymentMethodById), new { id = paymentMethodModel.UserPaymentMethodId }, paymentMethodModel.ToPaymentMethodDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdatePaymentMethod([FromRoute] int id, [FromBody] UpdatePaymentMethodDto paymentMethodDto)
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
            if (currUser == null)
            {
                return NotFound($"User details not found {currUserEmail}");
            }
            var updatingPaymentMethodModel = await _context.UserPaymentMethods.FirstOrDefaultAsync(pm => pm.UserPaymentMethodId == id);
            if (updatingPaymentMethodModel.UserId != currUser.UserId)
            {
                return Unauthorized($"User has no access to update");
            }
            var paymentMethodModel = await _paymentMethodRepo.UpdateAsync(id, paymentMethodDto);
            if (paymentMethodModel == null)
            {
                return NotFound();
            }
            return Ok(paymentMethodModel.ToPaymentMethodDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeletePaymentMethod([FromRoute] int id)
        {
            var currUserEmail = GetCurrentUserEmail();
            if (string.IsNullOrEmpty(currUserEmail))
            {
                return Unauthorized($"User not found {currUserEmail}");
            }
            var currUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == currUserEmail);
            if (currUser == null)
            {
                return NotFound($"User details not found {currUserEmail}");
            }
            var deletingPaymentMethodModel = await _context.UserPaymentMethods.FirstOrDefaultAsync(pm => pm.UserPaymentMethodId == id);
            if (deletingPaymentMethodModel.UserId != currUser.UserId)
            {
                return Unauthorized($"User has no access to delete");
            }
            var paymentMethodModel = await _paymentMethodRepo.DeleteAsync(id);
            if (paymentMethodModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
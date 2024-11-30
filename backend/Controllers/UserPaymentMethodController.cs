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
using backend.Helpers;

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
        private readonly UserHelper _userHelper;
        public UserPaymentMethodController(ApplicationDBContext context, IPaymentMethodRepository paymentMethodRepo, IUserRepository userRepo, UserHelper userHelper)
        {
            _paymentMethodRepo = paymentMethodRepo;
            _context = context;
            _userRepo = userRepo;
            _userHelper = userHelper;
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
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var paymentMethodModel = paymentMethodDto.ToUserPaymentMethodFromCreateDto(currUserId.Value);
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
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var updatingPaymentMethodModel = await _context.UserPaymentMethods.FirstOrDefaultAsync(pm => pm.UserPaymentMethodId == id);
            if (updatingPaymentMethodModel.UserId != currUserId.Value)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currUserId = await _userHelper.GetCurrentUserIdAsync(HttpContext);
            if (currUserId == null)
            {
                return Unauthorized("User not found.");
            }
            var deletingPaymentMethodModel = await _context.UserPaymentMethods.FirstOrDefaultAsync(pm => pm.UserPaymentMethodId == id);
            if (deletingPaymentMethodModel.UserId != currUserId.Value)
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
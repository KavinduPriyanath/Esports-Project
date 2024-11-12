using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.PaymentMethod;
using backend.Interfaces;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("backend/paymentmethod")]
    [ApiController]
    public class UserPaymentMethodController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IPaymentMethodRepository _paymentMethodRepo;
        public UserPaymentMethodController(ApplicationDBContext context, IPaymentMethodRepository paymentMethodRepo)
        {
            _paymentMethodRepo = paymentMethodRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaymentMethods()
        {
            var allPaymentMethods = await _paymentMethodRepo.GetAllAsync();
            var allPaymentMethodsDto = allPaymentMethods.Select(pm => pm.ToPaymentMethodDto());
            return Ok(allPaymentMethodsDto);
        }

        [HttpGet("{id}")]
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

        [HttpGet("user/{userId}")]
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

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] CreatePaymentMethodDto paymentMethodDto)
        {
            var paymentMethodModel = paymentMethodDto.ToUserPaymentMethodFromCreateDto();
            await _paymentMethodRepo.CreateAsync(paymentMethodModel);
            return CreatedAtAction(nameof(GetPaymentMethodById), new { id = paymentMethodModel.UserPaymentMethodId }, paymentMethodModel.ToPaymentMethodDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdatePaymentMethod([FromRoute] int id, [FromBody] UpdatePaymentMethodDto paymentMethodDto)
        {
            var paymentMethodModel = await _paymentMethodRepo.UpdateAsync(id, paymentMethodDto);
            if (paymentMethodModel == null)
            {
                return NotFound();
            }
            return Ok(paymentMethodModel.ToPaymentMethodDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeletePaymentMethod([FromRoute] int id)
        {
            var paymentMethodModel = await _paymentMethodRepo.DeleteAsync(id);
            if (paymentMethodModel == null)
            {
                return NotFound();
            }
            return NoContent(); 
        }
    }
}
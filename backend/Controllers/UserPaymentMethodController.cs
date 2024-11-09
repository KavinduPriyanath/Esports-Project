using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.PaymentMethod;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("backend/paymentmethod")]
    [ApiController]
    public class UserPaymentMethodController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserPaymentMethodController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllPaymentMethods()
        {
            var allPaymentMethods = _context.UserPaymentMethods.ToList().Select(pm => pm.ToPaymentMethodDto());
            return Ok(allPaymentMethods);
        }

        [HttpGet("{id}")]
        public IActionResult GetPaymentMethodById([FromRoute] int id)
        {
            var paymentMethodDetails = _context.UserPaymentMethods.Find(id);
            if (paymentMethodDetails == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(paymentMethodDetails);
            }
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetPaymentMethodsByUserId([FromRoute] int userId)
        {
            var paymentMethods = _context.UserPaymentMethods.Where(pm => pm.UserId == userId).ToList();
            if (!paymentMethods.Any())
            {
                return NotFound();
            }
            else
            {
                return Ok(paymentMethods);
            }
        }

        [HttpPost]
        public IActionResult CreatePaymentMethod([FromBody] CreatePaymentMethodDto paymentMethodDto)
        {
            var paymentMethodModel = paymentMethodDto.ToUserPaymentMethodFromCreateDto();
            _context.UserPaymentMethods.Add(paymentMethodModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPaymentMethodById), new {id = paymentMethodModel.UserPaymentMethodId}, paymentMethodModel.ToPaymentMethodDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdatePaymentMethod([FromRoute] int id, [FromBody] UpdatePaymentMethodDto paymentMethodDto)
        {
            var paymentMethodModel = _context.UserPaymentMethods.FirstOrDefault(pm => pm.UserPaymentMethodId == id);
            if (paymentMethodModel == null)
            {
                return NotFound();
            }  
            paymentMethodModel.CardNumber = paymentMethodDto.CardNumber;
            paymentMethodModel.CardType = paymentMethodDto.CardType;
            paymentMethodModel.BankName = paymentMethodDto.BankName;
            paymentMethodModel.BranchName = paymentMethodDto.BranchName;
            paymentMethodModel.BranchCode = paymentMethodDto.BranchCode;
            paymentMethodModel.IssuedDate = paymentMethodDto.IssuedDate;
            paymentMethodModel.ExpireDate = paymentMethodDto.ExpireDate;
            paymentMethodModel.CardStatus = paymentMethodDto.CardStatus;
            paymentMethodModel.CardVerified = paymentMethodDto.CardVerified;
            _context.SaveChanges();
            return Ok(paymentMethodModel.ToPaymentMethodDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeletePaymentMethod([FromRoute] int id)
        {
            var paymentMethodModel = _context.UserPaymentMethods.FirstOrDefault(pm => pm.UserPaymentMethodId == id);
            if (paymentMethodModel == null)
            {
                return NotFound();
            } 
            _context.UserPaymentMethods.Remove(paymentMethodModel);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
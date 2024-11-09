using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
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
            var allPaymentMethods = _context.UserPaymentMethods.ToList();
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

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.PaymentMethod
{
    public class PaymentMethodDto
    {
        public int UserPaymentMethodId { get; set; }
        public required string CardType { get; set; } // Credit, Debit
        public required string BankName { get; set; }
        public int CardStatus { get; set; } = 1; // Active=1, Inactive=0, Deleted=-1, Suspended=-2, Expired=-3 etc...
        public int? UserId { get; set; }
    }
}
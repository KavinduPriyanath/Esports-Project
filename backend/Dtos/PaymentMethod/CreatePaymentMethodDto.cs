using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.PaymentMethod
{
    public class CreatePaymentMethodDto
    {
        [Required]
        [Range(0, 99999999999999999, ErrorMessage = "Card Number should contain 16 digits")]
        // [Required]
        // [StringLength(16, MinimumLength = 16, ErrorMessage = "Card Number should contain exactly 16 digits")]
        // [RegularExpression(@"^\d{16}$", ErrorMessage = "Card Number should contain only digits")]
        // public required string CardNumber { get; set; }
        public required int CardNumber { get; set; }
        public required string CardType { get; set; } // Credit, Debit
        public required string BankName { get; set; }
        public required string BranchName { get; set; }
        // [Required]
        // [Length(3, 3, ErrorMessage = "Branch Code should contain 3 digits")]
        public required int BranchCode { get; set; }
        public required DateOnly IssuedDate { get; set; }
        public required DateOnly ExpireDate { get; set; }
        public int CardStatus { get; set; } = 1; // Active=1, Inactive=0, Deleted=-1, Suspended=-2, Expired=-3 etc...
        public bool CardVerified { get; set; } = false;
        public int? UserId { get; set; }
    }
}
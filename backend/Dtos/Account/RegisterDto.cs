using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string Nic { get; set; }
        public required DateOnly Dob { get; set; }
        public required string ContactNo { get; set; }
        public string Address { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;
        public string WorkingStatus { get; set; } = string.Empty;
    }
}
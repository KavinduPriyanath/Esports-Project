using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public required string UserEmail { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;
        public int UserAccountStatus { get; set; } = 1; // Active=1, Inactive=0, Deleted=-1, Suspended=-2 etc...
    }
}
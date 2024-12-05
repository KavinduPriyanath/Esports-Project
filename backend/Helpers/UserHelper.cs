using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Data;
using backend.Interfaces;
using backend.Models;
using backend.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Helpers
{
    public class UserHelper
    {
        private readonly ApplicationDBContext _context;
        private readonly IUserRepository _userRepo;

        public UserHelper(ApplicationDBContext context, IUserRepository userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }

        public async Task<int?> GetCurrentUserIdAsync(HttpContext httpContext)
        {
            var currUserEmail = httpContext?.User?.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(currUserEmail))
            {
                return null;
            }

            return await _context.Users.Where(u => u.UserEmail == currUserEmail).Select(u => u.UserId).FirstOrDefaultAsync() switch
            {
                0 => (int?)null,
                var userId => userId
            };
        }
    }
}
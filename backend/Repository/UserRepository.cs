using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.User;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public enum UserAccountStatus
        {
            Active = 1,
            Inactive = 0,
            Deleted = -1,
            Suspended = -2
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteAsync(int id)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(pm => pm.UserId == id);
            if (userModel == null)
            {
                return null;
            }
            userModel.UserAccountStatus = (int)UserAccountStatus.Deleted;
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<User?> DeactiveAsync(int id)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(pm => pm.UserId == id);
            if (userModel == null)
            {
                return null;
            }
            userModel.UserAccountStatus = (int)UserAccountStatus.Inactive;
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<User?> SuspendAsync(int id)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(pm => pm.UserId == id);
            if (userModel == null)
            {
                return null;
            }
            userModel.UserAccountStatus = (int)UserAccountStatus.Suspended;
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<User?> ReactivateAsync(int id)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(pm => pm.UserId == id);
            if (userModel == null)
            {
                return null;
            }
            userModel.UserAccountStatus = (int)UserAccountStatus.Active;
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<User?> UpdateAsync(int id, UpdateUserDto userDto)
        {
            var userModel = await _context.Users.FirstOrDefaultAsync(pm => pm.UserId == id);
            if (userModel == null)
            {
                return null;
            }
            userModel.UserId = userDto.UserId;
            userModel.FirstName = userDto.FirstName;
            userModel.LastName = userDto.LastName;
            userModel.UserName = userDto.UserName;
            userModel.UserEmail = userDto.UserEmail;
            userModel.Nic = userDto.Nic;
            userModel.Dob = userDto.Dob;
            userModel.ContactNo = userDto.ContactNo;
            userModel.Address = userDto.Address;
            userModel.ProfileImage = userDto.ProfileImage;
            userModel.WorkingStatus = userDto.WorkingStatus;
            userModel.UserAccountStatus = userDto.UserAccountStatus;
            await _context.SaveChangesAsync();
            return userModel;
        }

        public Task<bool> IsUserExist(int id)
        {
            return _context.Users.AnyAsync(u => u.UserId == id);
        }
    }
}
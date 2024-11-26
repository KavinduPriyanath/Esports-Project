using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos.PaymentMethod;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly ApplicationDBContext _context;
        public PaymentMethodRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<UserPaymentMethod>> GetAllAsync()
        {
            return await _context.UserPaymentMethods.ToListAsync();
        }

        public Task<List<UserPaymentMethod>> GetAllByIdAsync(int uid)
        {
            return _context.UserPaymentMethods.Where(pm => pm.UserId == uid).ToListAsync();
        }

        public async Task<UserPaymentMethod?> GetByIdAsync(int id)
        {
            return await _context.UserPaymentMethods.FindAsync(id);
        }

        public async Task<UserPaymentMethod> CreateAsync(UserPaymentMethod userPaymentMethod)
        {
            await _context.UserPaymentMethods.AddAsync(userPaymentMethod);
            await _context.SaveChangesAsync();
            return userPaymentMethod;
        }

        public async Task<UserPaymentMethod?> DeleteAsync(int id)
        {
            var paymentMethodModel = _context.UserPaymentMethods.FirstOrDefault(pm => pm.UserPaymentMethodId == id);
            if (paymentMethodModel == null)
            {
                return null;
            }
            _context.UserPaymentMethods.Remove(paymentMethodModel);
            await _context.SaveChangesAsync();
            return paymentMethodModel;
        }

        public async Task<UserPaymentMethod?> UpdateAsync(int id, UpdatePaymentMethodDto paymentMethodDto)
        {
            var paymentMethodModel = await _context.UserPaymentMethods.FirstOrDefaultAsync(pm => pm.UserPaymentMethodId == id);
            if (paymentMethodModel == null)
            {
                return null;
            }
            paymentMethodModel.UserPaymentMethodId = paymentMethodDto.UserPaymentMethodId;
            paymentMethodModel.CardNumber = paymentMethodDto.CardNumber;
            paymentMethodModel.CardType = paymentMethodDto.CardType;
            paymentMethodModel.BankName = paymentMethodDto.BankName;
            paymentMethodModel.BranchName = paymentMethodDto.BranchName;
            paymentMethodModel.BranchCode = paymentMethodDto.BranchCode;
            paymentMethodModel.IssuedDate = paymentMethodDto.IssuedDate;
            paymentMethodModel.ExpireDate = paymentMethodDto.ExpireDate;
            paymentMethodModel.CardStatus = paymentMethodDto.CardStatus;
            paymentMethodModel.CardVerified = paymentMethodDto.CardVerified;
            await _context.SaveChangesAsync();
            return paymentMethodModel;
        }


    }
}
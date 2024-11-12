using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.PaymentMethod;
using backend.Models;

namespace backend.Interfaces
{
    public interface IPaymentMethodRepository
    {
        Task<List<UserPaymentMethod>> GetAllAsync();
        Task<List<UserPaymentMethod>> GetAllByIdAsync(int uid);
        Task<UserPaymentMethod?> GetByIdAsync(int id);
        Task<UserPaymentMethod> CreateAsync(UserPaymentMethod userPaymentMethod);
        Task<UserPaymentMethod> UpdateAsync(int id, UpdatePaymentMethodDto paymentMethodDto);
        Task<UserPaymentMethod> DeleteAsync(int id);
    }
}
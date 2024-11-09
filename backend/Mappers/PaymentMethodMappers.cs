using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.PaymentMethod;
using backend.Models;

namespace backend.Mappers
{
    public static class PaymentMethodMappers
    {
        public static PaymentMethodDto ToPaymentMethodDto(this UserPaymentMethod paymentMethodModel)
        {
            return new PaymentMethodDto
            {
                UserPaymentMethodId = paymentMethodModel.UserPaymentMethodId,
                UserId = paymentMethodModel.UserId,
                CardType = paymentMethodModel.CardType,
                BankName = paymentMethodModel.BankName,
                CardStatus = paymentMethodModel.CardStatus
            };
        }

        public static UserPaymentMethod ToUserPaymentMethodFromCreateDto(this CreatePaymentMethodDto paymentMethodDto)
        {
            return new UserPaymentMethod 
            {
                CardNumber = paymentMethodDto.CardNumber,
                CardType = paymentMethodDto.CardType,
                BankName = paymentMethodDto.BankName,
                BranchName = paymentMethodDto.BranchName,
                BranchCode = paymentMethodDto.BranchCode,
                IssuedDate = paymentMethodDto.IssuedDate,
                ExpireDate = paymentMethodDto.ExpireDate,
                CardStatus = paymentMethodDto.CardStatus,
                CardVerified = paymentMethodDto.CardVerified,
                UserId = paymentMethodDto.UserId
            };
        }
    }
}
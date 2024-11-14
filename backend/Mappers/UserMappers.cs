using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.PaymentMethod;
using backend.Dtos.User;
using backend.Models;

namespace backend.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                ProfileImage = user.ProfileImage,
                UserAccountStatus = user.UserAccountStatus
            };
        }

        public static UserWithPaymentDto ToUserWithPaymentDto(this User user, List<PaymentMethodDto> paymentMethods)
        {
            return new UserWithPaymentDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                ProfileImage = user.ProfileImage,
                UserAccountStatus = user.UserAccountStatus,
                PaymentMethods = paymentMethods    // Get payment methods here
            };
        }

        public static User ToUserFromUserCreateDto(this CreateUserDto userDto)
        {
            return new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
                UserEmail = userDto.UserEmail,
                Nic = userDto.Nic,
                Dob = userDto.Dob,
                ContactNo = userDto.ContactNo,
                Address = userDto.Address,
                ProfileImage = userDto.ProfileImage,
                WorkingStatus = userDto.WorkingStatus
            };
        }
    }
}
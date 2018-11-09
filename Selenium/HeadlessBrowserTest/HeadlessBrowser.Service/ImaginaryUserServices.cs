using HeadlessBrowser.Common.Dto;
using HeadlessBrowser.Common.Enum;
using HeadlessBrowser.Common.Message;
using HeadlessBrowser.DataLayer;
using System;
using System.Collections.Generic;

namespace HeadlessBrowser.Service
{
    public class ImaginaryUserServices
    {
        public static ResponseMessage InsertUser(ImaginaryUserDto newUser)
        {
            ImaginaryUserDto result = ImaginaryUserDbOperations.Insert(newUser);
            return ResponseMessage.Ok(result);
        }

        public static ResponseMessage GetUsersByStatus(UserStatus status)
        {
            List<ImaginaryUserDto> result = ImaginaryUserDbOperations.GetUsersByStatus(status);
            return ResponseMessage.Ok(result);
        }

        public static ResponseMessage GetUsersByStatus(UserStatus status,int takeCount)
        {
            List<ImaginaryUserDto> result = ImaginaryUserDbOperations.GetUsersByStatus(status, takeCount);
            return ResponseMessage.Ok(result);
        }

        public static ResponseMessage UpdateUserStatus(long userId, UserStatus status)
        {
            ImaginaryUserDbOperations.UpdateUserStatus(userId, status);
            return ResponseMessage.Ok();
        }
        public static ResponseMessage BulkUpdateUserStatus(List<ImaginaryUserDto> users, UserStatus status)
        {
            ImaginaryUserDbOperations.BulkUpdateUserStatus(users, status);
            return ResponseMessage.Ok();
        }

        public static ResponseMessage UpdateUser(ImaginaryUserDto user)
        {
            ImaginaryUserDto result = ImaginaryUserDbOperations.Update(user);
            return ResponseMessage.Ok(result);
        }
    }
}

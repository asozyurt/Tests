using HeadlessBrowser.Common.Dto;
using HeadlessBrowser.Common.Enum;
using HeadlessBrowser.DataLayer;
using System.Collections.Generic;

namespace HeadlessBrowser.Service
{
    public class HeadlessBrowserServices
    {
        public static List<string> GetAllNames()
        {
            return TestsDbOperations.GetAllNames();
        }

        public static List<string> GetAllSurnames()
        {
            return TestsDbOperations.GetAllSurnames();
        }

        public static List<ImaginaryUserDto> GetUsersByStatus(UserStatus status)
        {
            return TestsDbOperations.GetUsersByStatus(status);
        }
        public static void UpdateUserStatus(long userId, UserStatus status)
        {
            TestsDbOperations.UpdateUserStatus(userId,status);
        }
        public static void BulkUpdateUserStatus(List<ImaginaryUserDto> users, UserStatus status)
        {
            TestsDbOperations.BulkUpdateUserStatus(users, status);
        }
    }
}

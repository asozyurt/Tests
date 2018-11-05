using HeadlessBrowser.Common;
using HeadlessBrowser.Common.Dto;
using HeadlessBrowser.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeadlessBrowser.DataLayer
{
    public class TestsDbOperations
    {
        public static List<string> GetAllNames()
        {
            List<string> result = new List<string>();
            using (TestDbEntities db = new TestDbEntities())
            {
                db.Names.ToList().ForEach(x => result.Add(x.Name));
            }
            return result;
        }

        public static List<string> GetAllSurnames()
        {
            List<string> result = new List<string>();
            using (TestDbEntities db = new TestDbEntities())
            {
                db.Surnames.ToList().ForEach(x => result.Add(x.Surname));
            }
            return result;
        }

        public static List<ImaginaryUserDto> GetUsersByStatus(UserStatus status)
        {
            List<ImaginaryUserDto> result = new List<ImaginaryUserDto>();

            using (TestDbEntities db = new TestDbEntities())
            {
                //TODO: Add AutoMapper
                db.ImaginaryUsers.Where(x => x.Status.Equals(status.ToString())).ToList().ForEach(t => result.Add(new ImaginaryUserDto
                {
                    Id = t.Id,
                    BirthDate = t.Birthdate ?? DateTime.MinValue,
                    CreateDate = t.CreateDate,
                    CreateUser = t.CreateUser,
                    Email = t.Email,
                    Gender = t.Gender,
                    Name = t.Name,
                    PhoneNumber = t.PhoneNumber,
                    Status = (UserStatus)Enum.Parse(typeof(UserStatus), t.Status),
                    Surname = t.Surname,
                    UpdateDate = t.UpdateDate,
                    UpdateUser = t.UpdateUser,
                    Username = t.Username
                }));
            }

            return result;
        }

        public static void UpdateUserStatus(long userId, UserStatus status)
        {
            BulkUpdateUserStatus(new List<ImaginaryUserDto> { new ImaginaryUserDto { Id = userId } }, status);
        }

        public static void BulkUpdateUserStatus(List<ImaginaryUserDto> users, UserStatus status)
        {
            DateTime now = DateTime.Now;

            using (TestDbEntities db = new TestDbEntities())
            {
                foreach (var user in users)
                {
                    var dbEntity = db.ImaginaryUsers.Find(user.Id);
                    dbEntity.Status = status.ToString();
                    dbEntity.UpdateDate = now;
                    dbEntity.UpdateUser = UserContext.CurrentUser;
                }
                db.SaveChanges();
            }
        }
    }
}

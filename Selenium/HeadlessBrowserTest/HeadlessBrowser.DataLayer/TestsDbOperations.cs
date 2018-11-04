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
                    CreateDate=t.CreateDate,
                    CreateUser =t.CreateUser,
                    Email =t.Email,
                    Gender =t.Gender,
                    Name=t.Name,
                    PhoneNumber =t.PhoneNumber,
                    Status = (UserStatus)Enum.Parse(typeof(UserStatus), t.Status),
                    Surname = t.Surname,
                    UpdateDate = t.UpdateDate,
                    UpdateUser = t.UpdateUser,
                    Username = t.Username
                }));
            }

            return result;
        }
    }
}

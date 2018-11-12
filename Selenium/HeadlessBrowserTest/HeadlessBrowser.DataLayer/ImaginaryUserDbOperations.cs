using HeadlessBrowser.Common;
using HeadlessBrowser.Common.Dto;
using HeadlessBrowser.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeadlessBrowser.DataLayer
{
    public class ImaginaryUserDbOperations
    {
        public static ImaginaryUserDto Insert(ImaginaryUserDto newUser)
        {
            newUser.SetBaseProperties();
            ImaginaryUsers dbRecord = new ImaginaryUsers
            {
                Username = newUser.Username,
                BirthDate = newUser.BirthDate,
                Email = newUser.Email,
                Gender = newUser.Gender,
                Name = newUser.Name,
                PhoneNumber = newUser.PhoneNumber,
                Status = newUser.Status.ToString(),
                Surname = newUser.Surname,
                Password = newUser.Password,
                IpAddress = newUser.IpAddress,
                Cookie = newUser.Cookie
            };
            MapToDbProperties(newUser, dbRecord);

            using (TestDbEntities db = new TestDbEntities())
            {
                db.ImaginaryUsers.Add(dbRecord);
                db.SaveChanges();
            }

            var result = new ImaginaryUserDto
            {
                BirthDate = dbRecord.BirthDate ?? DateTime.MinValue,
                Email = dbRecord.Email,
                Gender = dbRecord.Gender,
                Name = dbRecord.Name,
                PhoneNumber = dbRecord.PhoneNumber,
                Status = (UserStatus)Enum.Parse(typeof(UserStatus), dbRecord.Status),
                Surname = dbRecord.Surname,
                Username = dbRecord.Username,
                Password = dbRecord.Password,
                IpAddress = dbRecord.IpAddress,
                Cookie = dbRecord.Cookie
            };
            MapBaseProperties(dbRecord, result);

            return result;
        }

        public static ImaginaryUserDto Update(ImaginaryUserDto updatedUser)
        {
            updatedUser.SetBasePropertiesForUpdate();
            ImaginaryUsers original = null;
            using (TestDbEntities db = new TestDbEntities())
            {
                original = db.ImaginaryUsers.Find(updatedUser.Id);

                original.Status = updatedUser.Status.ToString();
                original.Username = updatedUser.Username;
                original.BirthDate = updatedUser.BirthDate;
                original.Email = updatedUser.Email;
                original.Gender = updatedUser.Gender;
                original.Name = updatedUser.Name;
                original.PhoneNumber = updatedUser.PhoneNumber;
                original.Surname = updatedUser.Surname;
                original.Password = updatedUser.Password;
                original.IpAddress = updatedUser.IpAddress;
                original.Cookie = updatedUser.Cookie;

                MapToDbProperties(updatedUser, original);

                db.SaveChanges();
            }

            var result = new ImaginaryUserDto
            {
                BirthDate = original.BirthDate ?? DateTime.MinValue,
                Email = original.Email,
                Gender = original.Gender,
                Name = original.Name,
                PhoneNumber = original.PhoneNumber,
                Status = (UserStatus)Enum.Parse(typeof(UserStatus), original.Status),
                Surname = original.Surname,
                Username = original.Username,
                Password = original.Password,
                IpAddress = original.IpAddress,
                Cookie = original.Cookie
            };
            MapBaseProperties(original, result);

            return result;
        }

        public static List<ImaginaryUserDto> GetUsersByStatus(UserStatus status, int takeCount = Constants.MAX_READ_COUNT)
        {
            List<ImaginaryUserDto> result = new List<ImaginaryUserDto>();

            using (TestDbEntities db = new TestDbEntities())
            {
                db.ImaginaryUsers.Where(x => x.Status.Equals(status.ToString())).Take(takeCount).OrderBy(x => x.Id).ToList().ForEach(t => result.Add(new ImaginaryUserDto
                {
                    Id = t.Id,
                    BirthDate = t.BirthDate ?? DateTime.MinValue,
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
                    Username = t.Username,
                    Password = t.Password,
                    IpAddress = t.IpAddress,
                    Cookie = t.Cookie
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
            using (TestDbEntities db = new TestDbEntities())
            {
                foreach (var user in users)
                {
                    user.SetBasePropertiesForUpdate();
                    var dbEntity = db.ImaginaryUsers.Find(user.Id);
                    dbEntity.Status = status.ToString();
                    dbEntity.UpdateDate = user.UpdateDate;
                    dbEntity.UpdateUser = user.UpdateUser;
                }
                db.SaveChanges();
            }
        }

        private static void MapBaseProperties(ImaginaryUsers source, ImaginaryUserDto target)
        {
            target.Id = source.Id;
            target.CreateDate = source.CreateDate;
            target.CreateUser = source.CreateUser;
            target.UpdateDate = source.UpdateDate;
            target.UpdateUser = source.UpdateUser;
        }
        private static void MapToDbProperties(ImaginaryUserDto sourceDto, ImaginaryUsers targetDbItem)
        {
            targetDbItem.CreateDate = sourceDto.CreateDate;
            targetDbItem.CreateUser = sourceDto.CreateUser;
            targetDbItem.UpdateDate = sourceDto.UpdateDate;
            targetDbItem.UpdateUser = sourceDto.UpdateUser;
        }
    }
}

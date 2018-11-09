using HeadlessBrowser.Common.Dto;
using HeadlessBrowser.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace HeadlessBrowser.Service
{
    public class SurnameDbOperations
    {
        public static SurnameDto GetById(long recordId)
        {
            SurnameDto result = null;
            using (TestDbEntities db = new TestDbEntities())
            {
                var dbRecord = db.Surnames.Find(recordId);
                if (dbRecord != null)
                {
                    result = new SurnameDto
                    {
                        Surname = dbRecord.Surname
                    };
                    MapBaseProperties(dbRecord, result);
                }
            }
            return result ?? default;
        }

        public static SurnameDto FindBySurname(string querySurname)
        {
            SurnameDto result = null;
            using (TestDbEntities db = new TestDbEntities())
            {
                var dbRecord = db.Surnames.FirstOrDefault(x => x.Surname.Equals(querySurname));
                if (dbRecord != null)
                {
                    result = new SurnameDto
                    {
                        Surname = dbRecord.Surname
                    };
                    MapBaseProperties(dbRecord, result);
                }
            }
            return result ?? default;
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

        private static void MapBaseProperties(Surnames sourceDbItem, SurnameDto targetDto)
        {
            targetDto.Id = sourceDbItem.Id;
            targetDto.CreateDate = sourceDbItem.CreateDate;
            targetDto.CreateUser = sourceDbItem.CreateUser;
            targetDto.UpdateDate = sourceDbItem.UpdateDate;
            targetDto.UpdateUser = sourceDbItem.UpdateUser;
        }
    }
}
using HeadlessBrowser.Common.Dto;
using HeadlessBrowser.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace HeadlessBrowser.Service
{
    public class NameDbOperations
    {
        public static NameDto GetById(long recordId)
        {
            NameDto result = null;
            using (TestDbEntities db = new TestDbEntities())
            {
                var dbRecord = db.Names.Find(recordId);
                if (dbRecord != null)
                {
                    result = new NameDto
                    {
                        Name = dbRecord.Name
                    };
                    MapBaseProperties(dbRecord, result);
                }
            }
            return result ?? default;
        }

        public static NameDto FindByName(string queryName)
        {
            NameDto result = null;
            using (TestDbEntities db = new TestDbEntities())
            {
                var dbRecord = db.Names.FirstOrDefault(x => x.Name.Equals(queryName));
                if (dbRecord != null)
                {
                    result = new NameDto
                    {
                        Name = dbRecord.Name
                    };
                    MapBaseProperties(dbRecord, result);
                }
            }
            return result ?? default;
        }

        public static List<string> GetAllNames()
        {
            List<string> result = new List<string>();
            using (TestDbEntities db = new TestDbEntities())
            {
                db.Names.ToList().ForEach(x => result.Add(x.Name));
            }
            return result;
        }

        private static void MapBaseProperties(Names sourceDbItem, NameDto targetDto)
        {
            targetDto.Id = sourceDbItem.Id;
            targetDto.CreateDate = sourceDbItem.CreateDate;
            targetDto.CreateUser = sourceDbItem.CreateUser;
            targetDto.UpdateDate = sourceDbItem.UpdateDate;
            targetDto.UpdateUser = sourceDbItem.UpdateUser;
        }
    }
}
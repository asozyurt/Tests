using HeadlessBrowser.Common.Message;
using System.Collections.Generic;

namespace HeadlessBrowser.Service
{
    public class SurnameServices
    {
        public static ResponseMessage GetSurnameById(long recordId)
        {
            var result = SurnameDbOperations.GetById(recordId);
            if (result != null && result.Id > 0)
            {
                return ResponseMessage.Ok(result);
            }
            return ResponseMessage.NOk();
        }

        public static ResponseMessage FindSurname(string querySurname)
        {
            var result = SurnameDbOperations.FindBySurname(querySurname);
            if (result != null && result.Id > 0)
            {
                return ResponseMessage.Ok(result);
            }
            return ResponseMessage.NOk();
        }

        public static ResponseMessage GetAllSurnames()
        {
            List<string> result = SurnameDbOperations.GetAllSurnames();
            if (result != null)
            {
                return ResponseMessage.Ok(result);
            }
            return ResponseMessage.NOk();
        }
    }
}

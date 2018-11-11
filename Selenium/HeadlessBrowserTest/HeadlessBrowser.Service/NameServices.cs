using HeadlessBrowser.Common.Dto;
using HeadlessBrowser.Common.Message;
using System;
using System.Collections.Generic;

namespace HeadlessBrowser.Service
{
    public class NameServices
    {
        public static ResponseMessage GetNameById(long recordId)
        {
            var result = NameDbOperations.GetById(recordId);
            if (result != null && result.Id > 0)
            {
                return ResponseMessage.Ok(result);
            }
            return ResponseMessage.NOk();
        }

        public static ResponseMessage FindName(string queryName)
        {
            var result = NameDbOperations.FindByName(queryName);
            if (result != null && result.Id > 0)
            {
                return ResponseMessage.Ok(result);
            }
            return ResponseMessage.NOk();
        }

        public static ResponseMessage GetAllNames()
        {
            List<string> result = NameDbOperations.GetAllNames();
            if (result != null)
            {
                return ResponseMessage.Ok(result);
            }
            return ResponseMessage.NOk();
        }

        public static ResponseMessage Insert(NameDto newRecord)
        {
            throw new NotImplementedException();
        }

        public static ResponseMessage Update(NameDto newRecord)
        {
            throw new NotImplementedException();
        }
    }
}

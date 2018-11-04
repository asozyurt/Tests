using System;

namespace HeadlessBrowser.Common.Dto
{
    public class BaseDto
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}

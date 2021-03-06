﻿using HeadlessBrowser.Common.Enum;
using System;

namespace HeadlessBrowser.Common.Dto
{
    public class ImaginaryUserDto : BaseDto
    {
        public UserStatus Status { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string Cookie { get; set; }
        public override string ToString()
        {
            return base.ToString();
            // TODO: Format Line
        }
    }

}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testproj.DTOs
{
    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public static explicit operator string(LoginDTO v)
        {
            throw new NotImplementedException();
        }
    }
}

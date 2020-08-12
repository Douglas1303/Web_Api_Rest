﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Api_Macorrati.DTOs
{
    public class UsuarioToken
    {
        public bool Authenticated { get; set; }
        public DateTime Expiration { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
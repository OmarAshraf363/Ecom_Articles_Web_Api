﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTO
{
   public class UpdateUserDTO
    {
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
   
        public IFormFile? PicImage { get; set; }

    }
}

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
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PicImage { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
    }
}

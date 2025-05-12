using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTO
{
   public class ResetPasswordDTO
    {
        public string NewPassword { get; set; }
        //public string ConfirmNewPassword { get; set; }

        public string? Token { get; set; }
        public string? Email { get; set; }


    }
}

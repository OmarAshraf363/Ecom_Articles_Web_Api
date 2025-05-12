using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Services
{
   public  interface IEmailSender
    {
        public void SendEmail(string email,string subject);
    }
}

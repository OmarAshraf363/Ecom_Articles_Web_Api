using Ecom.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositores.Service
{
    public class EmailSender : IEmailSender
    {
        public void SendEmail(string email, string subject)
        {
			try
			{
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("HotLine@gmail.com");
                mail.Subject = "Reset Password";
                mail.Body = subject;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com ",
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("oa38150@gmail.com", "deeo bpmm pmty pzcf"),
                    EnableSsl = true,

                };
                smtp.Send(mail);
            }
			catch (Exception)
			{

				throw;
			}
        }
    }
}

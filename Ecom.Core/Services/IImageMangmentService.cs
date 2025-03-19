using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Ecom.Core.Services
{
   public interface IImageMangmentService
    {
      public  Task<string> UploadImageAsync(IFormFile file , string src);
       public void DeleteImageAync(string src);
    }
}

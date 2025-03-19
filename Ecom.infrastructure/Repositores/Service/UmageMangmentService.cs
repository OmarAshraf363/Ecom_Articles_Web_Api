using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositores.Service
{
    public class UmageMangmentService : IImageMangmentService
    {
        private readonly IFileProvider fileProvider;
        public UmageMangmentService(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }
        public void DeleteImageAync(string src)
        {
            if (string.IsNullOrEmpty(src)) return;

            var folderPath = Path.Combine("wwwroot","Images", src);
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true); 
            }
        }




        public async Task<string> UploadImageAsync(IFormFile file, string src)
        {
            string finalSrc = null;
            var imageDirectory = Path.Combine("wwwroot", "Images", src);
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }
            if (file.Length > 0)
            {

                var imageName = file.FileName;
                var imageSrc = $"/Images/{src}/{imageName}";
                var root = Path.Combine(imageDirectory, file.FileName);
                using (var fileStream = new FileStream(root, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                finalSrc = imageSrc;
            }
            return finalSrc;
        }
    }
}

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
        public  void DeleteImageAync(string src)
        {
            if (string.IsNullOrEmpty(src)) 
                return;

            var imagePath =  Path.Combine("wwwroot", src.TrimStart('/'));

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }





        public async Task<string> UploadImageAsync(IFormFile file, string src)
        {
            if (file == null || file.Length == 0)
                return null;

            var imageDirectory = Path.Combine("wwwroot", "Images", src);

            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Unsupported file type.");
            }

            var uniqueName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(imageDirectory, uniqueName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageSrc = $"/Images/{src}/{uniqueName}";
            return imageSrc;
        }

    }
}

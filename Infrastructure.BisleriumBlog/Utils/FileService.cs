using Application.BisleriumBlog.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.BisleriumBlog.Utils
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment _environment;
        public FileService(IHostEnvironment environment)
        {
            _environment = environment;
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var contentPath = _environment.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = $"Only {string.Join(",", allowedExtensions)} extensions are allowed";
                    return new Tuple<int, string>(0, msg);
                }

                string uniqueString = Guid.NewGuid().ToString();
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);

                using (var stream = new FileStream(fileWithPath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "An error occurred while saving the image.");
            }
        }

        public async Task DeleteImage(string imageFileName)
        {
            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads", imageFileName);
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBlog.Utils
{
    public interface IFileService
    {
        public Tuple<int, string> SaveImage(IFormFile imageFile);
        public Task DeleteImage(string imageFileName);
    }
}

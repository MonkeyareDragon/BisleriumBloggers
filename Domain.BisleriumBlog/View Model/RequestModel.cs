using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.View_Model
{
    public class RequestModel
    {
        public class PostRequestModel
        {
            public string? Title { get; set; }
            public string? Content { get; set; }
            public string? ImageUrl { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.View_Model
{
    public class DashboardModels
    {
        public class DashboardCounts
        {
            public int TotalUsers { get; set; }
            public int TotalPosts { get; set; }
            public int TotalComments { get; set; }
            public int TotalVotes { get; set; }
        }
    }
}

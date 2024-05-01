using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.BisleriumBlog.View_Model.DashboardModels;

namespace Application.BisleriumBlog
{
    public interface IDashboardService
    {
        Task<DashboardCounts> GetDashboardCounts();
    }
}

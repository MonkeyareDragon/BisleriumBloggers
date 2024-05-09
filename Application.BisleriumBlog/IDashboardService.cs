using Domain.BisleriumBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.BisleriumBlog.View_Model.DashboardModels;
using static Domain.BisleriumBlog.View_Model.SendViewModels;

namespace Application.BisleriumBlog
{
    public interface IDashboardService
    {
        Task<DashboardCounts> GetDashboardCounts();
        Task<DashboardCounts> GetDashboardCountsOnChoosenTime(DateTime startDate, DateTime endDate);
        Task<List<PostSummaryDTO>> GetMostPopularPostsAllTime();
        Task<List<PostSummaryDTO>> GetMostPopularPostsChosenMonth(int month);
        Task<List<BloggerSummaryDTO>> GetMostPopularBloggersAllTime();
        Task<List<AppUser>> GetMostPopularBloggersChosenMonth(int month);
    }
}

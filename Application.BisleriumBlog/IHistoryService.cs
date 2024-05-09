using Domain.BisleriumBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBlog
{
    public interface IHistoryService
    {
        Task<History> AddHistory(string userId, Guid? postId, Guid? commentId, string previousContent, string UpdatedContent);
        Task<IEnumerable<History>> GetAllHistory(string userId);
    }
}

using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IFeedBackService
    {
        
        public Task<Pagination<FeedBackModel>> GetAllFeedBackPagingAsync(PaginationParameter paginationParameter);

        public Task<FeedBackModel?> GetFeedBackByIdAsync(int id);

        public Task<bool?> AddFeedBackAsync(FeedBackModel model);

        public Task<bool> RemoveFeedBackByIdAsync(int id, int userId);

        public Task<FeedBackModel> UpdateFeedBackAsync(FeedBackModel model);

        public Task<double> AverageFeedBackInProduct(int productId);

        public Task<Pagination<FeedBackModel>> GetFeedBackByProductIdAsync(int productId, PaginationParameter paginationParameter);

        public Task<int> CountFeedBackByProductAsync(int Id);
    }
}

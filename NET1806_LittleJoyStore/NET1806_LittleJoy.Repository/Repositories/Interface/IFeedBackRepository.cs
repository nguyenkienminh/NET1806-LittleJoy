using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IFeedBackRepository
    {
        
        public Task<Pagination<Feedback>> GetAllFeedBackPagingAsync(PaginationParameter paginationParameter);

        public Task<Feedback?> GetFeedBackByIdAsync(int id);

        public Task<bool?> AddFeedBackAsync(Feedback feedback);

        public Task<bool> RemoveFeedBackAsync(Feedback feedback);

        public Task<Feedback> UpdateFeedBackAsync(Feedback feedbackModify, Feedback feedbackPlace);

        public Task<Pagination<Feedback>> FeedBackInProductAsync(int productId, PaginationParameter paginationParameter);

        public Task<double> AverageRatingAsync(int productId);
    }
}

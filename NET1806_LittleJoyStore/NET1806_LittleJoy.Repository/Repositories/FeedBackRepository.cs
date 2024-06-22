using Microsoft.EntityFrameworkCore;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories
{
    public class FeedBackRepository : IFeedBackRepository
    {
        private readonly LittleJoyContext _context;

        public FeedBackRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Feedback>> GetAllFeedBackPagingAsync(PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Feedbacks.CountAsync();

            var item = await _context.Feedbacks.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                            .Take(paginationParameter.PageSize)
                                            .AsNoTracking()
                                            .ToListAsync();

            var result = new Pagination<Feedback>(item, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<Feedback?> GetFeedBackByIdAsync(int id)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool?> AddFeedBackAsync(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveFeedBackAsync(Feedback feedback)
        {
            _context.Feedbacks.Remove(feedback);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Feedback> UpdateFeedBackAsync(Feedback feedbackModify)
        {
            
            _context.Feedbacks.Update(feedbackModify);
            await _context.SaveChangesAsync();
            return feedbackModify;
        }

        public async Task<Pagination<Feedback>> FeedBackInProductAsync(int productId, PaginationParameter paginationParameter)
        {
            var itemCount = await _context.Feedbacks.CountAsync(f => f.ProductId == productId);

            var item = await _context.Feedbacks.Where(f => f.ProductId == productId)
                                            .OrderByDescending(f => f.Rating)
                                            .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                            .Take(paginationParameter.PageSize)
                                            .AsNoTracking()
                                            .ToListAsync();

            var result = new Pagination<Feedback>(item, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<double> AverageRatingAsync(int productId)
        {
            var item =  _context.Feedbacks.Where(f => f.ProductId == productId).AsQueryable();

            var count = await item.CountAsync();

            if(count == 0)
            {
                return 0.0;
            }

            var total = (double) await item.SumAsync(f => f.Rating);

            return Math.Round( (double) total/count);
        }

        public async Task<int> CountFeedBackByProductAsync(int Id)
        {
            return await _context.Feedbacks.CountAsync(p => p.ProductId == Id);
        }
    }
}

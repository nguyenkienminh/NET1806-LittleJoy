using AutoMapper;
using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services
{
    public class FeedBackService : IFeedBackService
    {
        private readonly IFeedBackRepository _feedBackRepo;
        private readonly IMapper _mapper;

        public FeedBackService(IFeedBackRepository feedBackRepo, IMapper mapper)
        {
            _feedBackRepo = feedBackRepo;
            _mapper = mapper;
        }

        public async Task<Pagination<FeedBackModel>> GetAllFeedBackPagingAsync(PaginationParameter paginationParameter)
        {
            var list = await _feedBackRepo.GetAllFeedBackPagingAsync(paginationParameter);

            if (!list.Any())
            {
                return null;
            }

            var listModel = list.Select(f => new FeedBackModel
            {
                Id = f.Id,
                UserId = f.UserId,
                ProductId = f.ProductId,
                Comment = f.Comment,
                Rating = f.Rating,
                Date = f.Date
            }).ToList();


            return new Pagination<FeedBackModel>(listModel,
                list.TotalCount,
                list.CurrentPage,
                list.PageSize);
        }

        public async Task<FeedBackModel?> GetFeedBackByIdAsync(int id)
        {
            var detail = await _feedBackRepo.GetFeedBackByIdAsync(id);

            if (detail == null)
            {
                return null;
            }

            var detailModel = _mapper.Map<FeedBackModel>(detail);

            return detailModel;
        }

        public async Task<bool?> AddFeedBackAsync(FeedBackModel model)
        {
            try
            {

                if (model.Rating < 1 || model.Rating > 5)
                {
                    return false;
                }

                var feedback = _mapper.Map<Feedback>(model);

                feedback.Date = DateTime.UtcNow.AddHours(7);

                var item = await _feedBackRepo.AddFeedBackAsync(feedback);

                if (item == null)
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fail to create FeedBack {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveFeedBackByIdAsync(int id)
        {
            var remove = await _feedBackRepo.GetFeedBackByIdAsync(id);

            if (remove == null)
            {
                return false;
            }

            return await _feedBackRepo.RemoveFeedBackAsync(remove);
        }

        public async Task<FeedBackModel> UpdateFeedBackAsync(FeedBackModel model)
        {

            var feedBackPlace = await _feedBackRepo.GetFeedBackByIdAsync(model.Id);

            if (feedBackPlace == null)
            {
                return null;
            }

            Feedback feedBackModify = new Feedback()
            {
                Id = model.Id,
                ProductId = feedBackPlace.ProductId,
                UserId = feedBackPlace.UserId,
                Comment = model.Comment,
                Rating = model.Rating,
                Date = DateTime.UtcNow.AddHours(7)
            };

            var updateFeedBack = await _feedBackRepo.UpdateFeedBackAsync(feedBackModify, feedBackPlace);

            if (updateFeedBack != null)
            {
                return _mapper.Map<FeedBackModel>(updateFeedBack);
            }
            return null;
        }

        public async Task<ProductAverageRating> AverageFeedBackInProduct(int productId)
        {

            var rating = await _feedBackRepo.AverageRatingAsync(productId);

            return new ProductAverageRating()
            {
                ProductId = productId,
                RatingAver = rating
            };
        }

        public async Task<Pagination<FeedBackModel>> GetFeedBackByProductIdAsync(int productId, PaginationParameter paginationParameter)
        {
            var list = await _feedBackRepo.FeedBackInProductAsync(productId,paginationParameter);

            if (!list.Any())
            {
                return null;
            }

            var listModel = list.Select(f => new FeedBackModel
            {
                Id = f.Id,
                UserId = f.UserId,
                ProductId = f.ProductId,
                Comment = f.Comment,
                Rating = f.Rating,
                Date = f.Date
            }).ToList();


            return new Pagination<FeedBackModel>(listModel,
                list.TotalCount,
                list.CurrentPage,
                list.PageSize);
        }
    }
}

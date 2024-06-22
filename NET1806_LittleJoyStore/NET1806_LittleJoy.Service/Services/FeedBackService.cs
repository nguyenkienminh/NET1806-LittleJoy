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
        private readonly IUserService _userService;
        private readonly IProductRepositoty _productRepositoty;

        public FeedBackService(IFeedBackRepository feedBackRepo, IMapper mapper, IUserService userService, IProductRepositoty productRepositoty)
        {
            _feedBackRepo = feedBackRepo;
            _mapper = mapper;
            _userService = userService;
            _productRepositoty = productRepositoty;
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

        public async Task<bool> RemoveFeedBackByIdAsync(int id, int UserId)
        {
            var remove = await _feedBackRepo.GetFeedBackByIdAsync(id);

            if (remove == null)
            {
                return false;
            }
            else
            {
                var user = await _userService.GetUserByIdAsync(remove.UserId); // lay user tu feedback trong he thong

                if (user != null)
                {
                    if (user.Id != UserId)
                    {
                        return false;
                    }
                    else
                    {
                        return await _feedBackRepo.RemoveFeedBackAsync(remove);
                    }
                }
  
            }
            return false;
                    
        }

        public async Task<FeedBackModel> UpdateFeedBackAsync(FeedBackModel model)
        {

            var feedBackPlace = await _feedBackRepo.GetFeedBackByIdAsync(model.Id);

            if (feedBackPlace == null)
            {
                return null;
            }
            else
            {

                var user = await _userService.GetUserByIdAsync(feedBackPlace.UserId); // lay user tu feedback trong he thong

                if (user == null)
                {
                    return null;
                }
                else
                {
                    if (user.Id != model.UserId)
                    {
                        return null;
                    }
                    else 
                    {
                        Feedback feedBackModify = new Feedback()
                        {
                            Id = model.Id,
                            ProductId = feedBackPlace.ProductId,
                            UserId = model.UserId,
                            Comment = model.Comment,
                            Rating = model.Rating,
                            Date = DateTime.UtcNow.AddHours(7)
                        };

                        feedBackPlace.Id = feedBackModify.Id;
                        feedBackPlace.UserId = feedBackModify.UserId;
                        feedBackPlace.ProductId = feedBackModify.ProductId;
                        feedBackPlace.Comment = feedBackModify.Comment;
                        feedBackPlace.Rating = feedBackModify.Rating;
                        feedBackPlace.Date = feedBackModify.Date;

                        var updateFeedBack = await _feedBackRepo.UpdateFeedBackAsync(feedBackPlace);

                        if (updateFeedBack != null)
                        {
                            return _mapper.Map<FeedBackModel>(updateFeedBack);
                        }
                    }
                }
            }

            
            return null;
        }

        public async Task<double> AverageFeedBackInProduct(int productId)
        {

            return await _feedBackRepo.AverageRatingAsync(productId);
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

        public async Task<int> CountFeedBackByProductAsync(int Id)
        {
            try
            {
                var item = await _productRepositoty.GetProductByIdAsync(Id);

                if (item == null)
                {
                    throw new Exception("Product không tồn tại");
                }

                return await _feedBackRepo.CountFeedBackByProductAsync(Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

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
        private readonly IOrderRepository _orderRepository;

        public FeedBackService(IFeedBackRepository feedBackRepo, IMapper mapper, IUserService userService, IProductRepositoty productRepositoty, IOrderRepository orderRepository)
        {
            _feedBackRepo = feedBackRepo;
            _mapper = mapper;
            _userService = userService;
            _productRepositoty = productRepositoty;
            _orderRepository = orderRepository;
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
                var product = await _productRepositoty.GetProductByIdAsync(model.ProductId);

                if (product == null)
                {
                    throw new Exception($"Sản phẩm không tồn tại");
                }

                var checkBuy = await CheckProductHasBuyByUser(model);

                if (checkBuy == false)
                {

                    if (product != null)
                    {
                        throw new Exception($"Người dùng chưa mua sản phẩm đó nên không thể tạo feedback");
                    }
                    else
                    {
                        throw new Exception($"Product không tồn tại");
                    }
                }
                else
                {

                    if (!model.Rating.HasValue)
                    {
                        throw new Exception("Rating không được trống");
                    }

                    if (model.Rating < 1 || model.Rating > 5)
                    {
                        throw new Exception("Sai số rating");
                    }

                    var feedback = _mapper.Map<Feedback>(model);

                    feedback.Date = DateTime.UtcNow.AddHours(7);

                    var item = await _feedBackRepo.AddFeedBackAsync(feedback);

                    if (item == null)
                    {
                        return false;
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CheckProductHasBuyByUser(FeedBackModel model)
        {
            try
            {
                #region check người dùng đã mua hàng đó chưa
                var user = await _userService.GetUserByIdAsync(model.UserId); // lay user  trong he thong

                if (user == null)
                {
                    throw new Exception("User is not exist");
                }
                else
                {
                    var listOrder = await _orderRepository.GetOrderListByUserIdAsync(user.Id); // lay danh sach don hang cua nguoi dung

                    if (listOrder.Any())
                    {
                        foreach (var orders in listOrder)
                        {

                            if(orders.DeliveryStatus != null) {

                                if (orders.DeliveryStatus.Equals("Giao hàng thành công"))
                                {

                                    var OrderDetail = await _orderRepository.GetOrderDetailsByOrderId(orders.Id); //lay chi tiet don hang cua tung don hang

                                    if (OrderDetail.Any())
                                    {
                                        foreach (var detail in OrderDetail)
                                        {
                                            if (detail.ProductId == model.ProductId)
                                            {
                                                return true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Đơn hàng không có chi tiết bên trong");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Người dùng chưa mua hàng");
                    }
                }

                return false;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RemoveFeedBackByIdAsync(int id, int UserId)
        {
            try
            {
                var remove = await _feedBackRepo.GetFeedBackByIdAsync(id);

                if (remove == null)
                {
                    throw new Exception("Feedback is not exist");
                }
                else
                {
                    var user = await _userService.GetUserByIdAsync(remove.UserId); // lay user tu feedback trong he thong

                    if (user != null)
                    {
                        if (user.Id != UserId)
                        {
                            throw new Exception("Feedback of user is not exist");
                        }
                        else
                        {
                            return await _feedBackRepo.RemoveFeedBackAsync(remove);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;

        }

        public async Task<FeedBackModel> UpdateFeedBackAsync(FeedBackModel model)
        {
            try
            {
                var feedBackPlace = await _feedBackRepo.GetFeedBackByIdAsync(model.Id);
                if (!model.Rating.HasValue)
                {
                    throw new Exception("Rating is empty");
                }

                if (feedBackPlace == null)
                {
                    throw new Exception("Feedback is not exist");
                }
                else
                {

                    var user = await _userService.GetUserByIdAsync(feedBackPlace.UserId); // lay user tu feedback trong he thong

                    if (user == null)
                    {
                        throw new Exception("Feedback of user is not exist");
                    }
                    else
                    {
                        if (user.Id != model.UserId)
                        {
                            throw new Exception("Feedback of user is not exist");
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
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return null;
        }

        public async Task<double> AverageFeedBackInProduct(int productId)
        {

            return await _feedBackRepo.AverageRatingAsync(productId);
        }

        public async Task<Pagination<FeedBackModel>> GetFeedBackByProductIdAsync(int productId, PaginationParameter paginationParameter)
        {
            var list = await _feedBackRepo.FeedBackInProductAsync(productId, paginationParameter);

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

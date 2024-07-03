using NET1806_LittleJoy.Repository.Commons;
using NET1806_LittleJoy.Repository.Repositories;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services
{
    public class PointMoneyService : IPointMoneyService
    {
        private readonly IPointsMoneyRepository _pointsMoneyRepository;
        private readonly IUserRepository _userRepository;

        public PointMoneyService(IPointsMoneyRepository pointsMoneyRepository, IUserRepository userRepository) 
        {
            _pointsMoneyRepository = pointsMoneyRepository;
            _userRepository = userRepository;
        }
        public async Task<List<PointMoneyResponseModel>> CheckDiscount(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if(user == null)
            {
                throw new Exception("Không tìm thấy user");
            }
            var points = await _pointsMoneyRepository.GetAll();
            var userPoints = user.Points;
            var list = points.Select(p => new PointMoneyResponseModel()
            {
                Id = p.Id,
                AmountDiscount = p.AmountDiscount,
                MinPoints = p.MinPoints,
            }).ToList();
            foreach (var item in list) 
            {
                if(userPoints > item.MinPoints)
                {
                    item.CanDiscount = true;
                }
                else
                {
                    item.CanDiscount = false;
                }
            }

            return list;
        }
    }
}

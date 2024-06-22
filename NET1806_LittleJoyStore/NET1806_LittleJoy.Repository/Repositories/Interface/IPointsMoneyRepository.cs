using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IPointsMoneyRepository
    {
        public Task<PointMoney> GetPointsByMoneyDiscount(int? amount);
    }
}

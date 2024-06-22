using Microsoft.EntityFrameworkCore;
using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories
{
    public class PointsMoneyRepository : IPointsMoneyRepository
    {
        private readonly LittleJoyContext _context;

        public PointsMoneyRepository(LittleJoyContext context) 
        {
            _context = context;
        }
        public async Task<PointMoney> GetPointsByMoneyDiscount(int? amount)
        {
            return await _context.PointMoneys.Where(x => x.AmountDiscount == amount).FirstOrDefaultAsync();
        }
    }
}

using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly LittleJoyContext _context;

        public PaymentRepository(LittleJoyContext context) 
        {
            _context = context;
        }

        public async Task<int> CreateNewPayment(int orderid)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateNewPayment()
        {
            throw new NotImplementedException();
        }
    }
}

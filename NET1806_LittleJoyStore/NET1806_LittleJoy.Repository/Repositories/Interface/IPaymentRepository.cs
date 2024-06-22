using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IPaymentRepository
    {
        public Task<bool> CreateNewPayment(Payment payment);

        public Task<Payment?> GetPaymentByOrderCode(int orderCode);

        public Task<Payment> UpdatePayment(Payment payment);

        public Task<Payment> GetPaymentByOrderId(int orderId);
    }
}

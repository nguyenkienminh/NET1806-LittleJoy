using NET1806_LittleJoy.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Repositories.Interface
{
    public interface IOtpRepository
    {
        public Task<Otp> CreateOtp(Otp otp);
        public Task<Otp> GetOtp(int code, string mail);
        public Task UpdateOtp(Otp otp);
    }
}

using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services.Interface
{
    public interface IOtpService
    {
        Task<Otp> AddNewOtp(string email);
        Task<bool> VerifyOtp(string email, int OtpCode);
    }
}

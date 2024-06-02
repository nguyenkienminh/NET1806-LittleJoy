using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
using NET1806_LittleJoy.Service.BusinessModels;
using NET1806_LittleJoy.Service.Services.Interface;
using NET1806_LittleJoy.Service.Ultils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;

        public OtpService(IOtpRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }
        public async Task<Otp> AddNewOtp(string email)
        {
            TimeZoneInfo utcPlus7 = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, utcPlus7);
            var otp = new Otp()
            {
                Email = email,
                OTPCode = NumberUltils.GenerateNumber(6),
                OTPTime = timeNow.AddMinutes(5),
            };
            await _otpRepository.CreateOtp(otp);
            return otp;
        }

        public async Task<bool> VerifyOtp(string email, int otpcode)
        {
            var otp = await _otpRepository.GetOtp(otpcode, email);
            if(otp !=  null)
            {
                otp.IsUsed = true;
                await _otpRepository.UpdateOtp(otp);
                return true;
            }
            return false;
        }
    }
}

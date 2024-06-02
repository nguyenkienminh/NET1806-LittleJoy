using NET1806_LittleJoy.Repository.Entities;
using NET1806_LittleJoy.Repository.Repositories.Interface;
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

        public async Task VerifyOtp(string mail, int OTPCode)
        {
            var result = await _otpRepository.GetOtp(OTPCode, mail);
            if(result == null)
            {
                throw new Exception("OTP sai hoặc đã hết hạn");
            }
        }
    }
}

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
    public class OtpRepository : IOtpRepository
    {
        private readonly LittleJoyContext _context;

        public OtpRepository(LittleJoyContext context)
        {
            _context = context;
        }

        public async Task<Otp> CreateOtp(Otp otp)
        {
            _context.Otps.Add(otp);
            await _context.SaveChangesAsync();
            return otp;
        }

        public async Task<Otp> GetOtp(int code, string mail)
        {
            TimeZoneInfo utcPlus7 = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, utcPlus7);
            return await _context.Otps.Where(x => x.OTPCode == code && x.OTPTime > timeNow && x.Email == mail && x.IsUsed == false).FirstOrDefaultAsync();
        }

        public async Task UpdateOtp(Otp otp)
        {
            _context.Otps.Update(otp);
            await _context.SaveChangesAsync();
        }
    }
}

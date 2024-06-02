using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Repository.Entities
{
    public partial class Otp
    {
        public int Id { get; set; }

        public string Email { get; set; } = "";

        public int OTPCode { get; set; } = 0;

        public bool IsUsed { get; set; } = false;

        public DateTime OTPTime { get; set; }

    }
}

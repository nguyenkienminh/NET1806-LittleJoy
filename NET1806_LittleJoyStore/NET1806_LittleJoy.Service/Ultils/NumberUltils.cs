using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Ultils
{
    public class NumberUltils
    {
        public static int GenerateNumber(int length)
        {
            if (length <= 0 || length > 10)
            {
                throw new ArgumentException("Length must be between 1 and 10");
            }

            Random random = new Random();
            string result = string.Empty;

            // Đảm bảo chữ số đầu tiên không phải là 0 nếu độ dài lớn hơn 1
            if (length > 1)
            {
                result += random.Next(1, 10).ToString();
                length--;
            }
            else
            {
                // Nếu độ dài là 1, có thể bắt đầu với 0
                return random.Next(0, 10);
            }

            for (int i = 0; i < length; i++)
            {
                result += random.Next(0, 10).ToString();
            }

            return int.Parse(result);
        }
    }
}

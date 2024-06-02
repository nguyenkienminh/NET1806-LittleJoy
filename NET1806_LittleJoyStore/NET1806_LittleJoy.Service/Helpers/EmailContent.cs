using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1806_LittleJoy.Service.Helpers
{
    public class EmailContent
    {
        public static string EmailOTPContent(string username, int otp)
        {
            return "<div style=\"background-color:#f8f8f8;font-family:sans-serif;padding:15px\">\n"
                + "    <div style=\"max-width:1000px;margin:auto\">\n"
                + "        <div class=\"adM\">\n"
                + "            <div style=\"background-color:#fff;padding:5px 20px;color:#000;border-radius:0px 0px 2px 2px\">\n"
                + "                <div style=\"padding:35px 15px\">\n"
                + "                    <p style=\"margin:0;font-size:16px\">\n"
                + "                        <b>Xin chào, " + username + "</b>\n"
                + "                    </p>\n"
                + "                    <br>\n"
                + "                    <p style=\"margin:0;font-size:16px\">\n"
                + "                        Bạn vừa nhận được mã\n"
                + "                        <span class=\"il\">OTP</span>\n"
                + "                        xác nhận tại\n"
                + "                        <a style=\"text-decoration:none\" href=\"#\" target=\"_blank\">Little Joy Store</a>\n"
                + "                    </p>\n"
                + "                    <div style=\"padding:40px;margin:auto;text-align:center\">\n"
                + "                        <div style=\"width:fit-content;border:#3cc892 thin solid;color:#3cc892;font-weight:bold;text-align:center;padding:7px 12px;border-radius:2px;margin:auto;font-size:large\">" + otp + "</div>\n"
                + "                    </div>\n"
                + "                    <div style=\"border-top:1px solid #dcdbdb\"></div>\n"
                + "                    <br>\n"
                + "                    <p style=\"margin:0;font-size:16px\">\n"
                + "                        Nếu bạn không thực hiện yêu cầu này, xin vui lòng bỏ qua nó hoặc nếu cần hỗ trợ, liên hệ với chúng tôi\n"
                + "                        <a style=\"text-decoration:none\" href=\"#\" target=\"_blank\">ngay</a>\n"
                + "                        .\n"
                + "                    </p>\n"
                + "                    <br>\n"
                + "                    <p style=\"margin:0;font-size:16px\">Trân trọng,</p>\n"
                + "                    <p style=\"margin:0;font-size:16px\">Little Joy Store Team.</p>\n"
                + "                </div>\n"
                + "            </div>\n"
                + "        </div>\n"
                + "    </div>\n"
                + "</div>";
        }
    }
}

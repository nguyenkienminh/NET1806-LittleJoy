using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public static string ConfirmEmail(string username, string tokenConfirm)
        {
            return "<div style=\"background-color:#f8f8f8;font-family:sans-serif;padding:15px\">\n"
                + "        <div style=\"max-width:1000px ; margin:auto\">\n"
                + "            <div class=\"adM\">\n"
                + "                <div style=\"background-color:#fff;padding:5px 20px;color:#000;border-radius:0px 0px 2px 2px\">\n"
                + "                    <div style=\"padding:35px 15px\">\n"
                + "                        <p style=\"margin:0;font-size:16px\">\n"
                + "                            <b>Xin chào, "+ username +" </b>\n"
                + "                        </p>\n"
                + "                        <br>\n"
                + "                        <p style=\"margin:0;font-size:16px\">\n"
                + "                            Bạn vừa đăng kí tài khoản, vui lòng ấn vào nút dưới đây để xác nhận tài khoản tại\n"
                + "                            <a style=\"text-decoration:none\\\\\" href=\"https://littlejoy.vercel.app\\\\\"\n"
                + "                                target=\"_blank\">Little Joy Store</a>\n"
                + "                        </p>\n"
                + "                        <div style=\"padding:40px;margin:auto;text-align:center\">\n"
                + "                            <a href=\"https://littlejoy.vercel.app/confirm/" + tokenConfirm +"\"\n"
                + "                                style=\"color: #3cc892; text-decoration: none;\">\n"
                + "                                <div\n"
                + "                                    style=\"width:fit-content;border:#3cc892 thin solid;color:#3cc892;font-weight:bold;text-align:center;padding:7px 12px;border-radius:2px;margin:auto;font-size:large\">\n"
                + "                                    XÁC NHẬN\n"
                + "                                </div>\n"
                + "                            </a>\n"
                + "                        </div>\n"
                + "                        <div style=\"border-top:1px solid #dcdbdb\"></div>\n"
                + "                        <br>\n"
                + "                        <p style=\"margin:0;font-size:16px\">Trân trọng,</p>\n"
                + "                        <p style=\"margin:0;font-size:16px\">Little Joy Store Team.</p>\n"
                + "                    </div>\n"
                + "                </div>\n"
                + "            </div>\n"
                + "        </div>\n"
                + "    </div>";
        }

        public static string WelcomeEmail(string username)
        {
            return "<div style=\"background-color:#f8f8f8;font-family:sans-serif;padding:15px\">\n"
                + "        <div style=\"max-width:1000px ; margin:auto\">\n"
                + "            <div class=\"adM\">\n"
                + "                <div style=\"background-color:#fff;padding:5px 20px;color:#000;border-radius:0px 0px 2px 2px\">\n"
                + "                    <div style=\"padding:35px 15px;\">\n"
                + "                        <div style=\"display: flex; justify-content: center; align-items: center;\">\n"
                + "                            <div style=\"width: 10%;\">\n"
                + "                                <a href=\"https://littlejoy.vercel.app/\"><img\n"
                + "                                    src=\"https://firebasestorage.googleapis.com/v0/b/little-joy-2c5d3.appspot.com/o/Logo%20Little%20Joy%20Store.png?alt=media&token=c0752ee7-f2c0-400e-9024-632160b7aa66\"\n"
                + "                                    alt=\"\" style=\"width: 100%\"></a>\n"
                + "                            </div>\n"
                + "                            <span style=\"font-size: 25px; color: #005B96; font-weight: 600; padding-left: 20px;\">Chào mừng bạn đến với\n"
                + "                                Little Joy Store</span>\n"
                + "                        </div>\n"
                + "                        <div style=\"margin-top: 20px; font-size: 16px;\">\n"
                + "                            <p><strong>Xin chào "+username+",</strong></p>\n"
                + "                            <span>Cảm ơn bạn đã đăng ký tài khoản tại <span style=\"color: #1A469E; font-weight: 600;\">Little Joy Store</span>. Chúng tôi rất vui mừng được chào đón bạn đến với cộng đồng của chúng tôi.</span><br>\n"
                + "                            <p style=\"margin-top: 15px; margin-bottom: 15px;\">Tại đây, bạn sẽ tìm thấy:</p>\n"
                + "                            <ul>\n"
                + "                                <li><strong>Các loại sữa chất lượng cao:</strong> Đảm bảo an toàn và dinh dưỡng cho cả mẹ và bé.</li>\n"
                + "                                <li><strong>Ưu đãi đặc biệt:</strong> Các ưu đã thường xuyên được cập nhật</li>\n"
                + "                                <li><strong>Tư vấn miễn phí:</strong> Giúp bạn chọn sản phẩm phù hợp nhất.</li>\n"
                + "                                <li>Và rất nhiều tiện ích khác ...</li>\n"
                + "                            </ul>\n"
                + "                            <p>Hãy bắt đầu khám phá ngay các sản phẩm và chương trình khuyến mãi hấp dẫn từ chúng tôi.</p>\n"
                + "                            <p>Nếu bạn cần bất kỳ sự hỗ trợ nào, vui lòng liên hệ với chúng tôi qua <a href=\"#\" style=\"text-decoration: none; color: #004574; font-weight: 600;\">Little Joy Team</a></p>\n"
                + "                            <p>Chúc bạn và gia đình luôn khỏe mạnh và hạnh phúc!</p>\n"
                + "                            <p style=\"margin:0;font-size:16px;font-weight: 600;\">Trân trọng,</p>\n"
                + "                            <p style=\"margin:0;font-size:16px;font-weight: 600;\">Little Joy Store Team.</p>\n"
                + "                        </div>\n"
                + "\n"
                + "                    </div>\n"
                + "                </div>\n"
                + "            </div>\n"
                + "        </div>\n"
                + "    </div>";

        }
    }
}

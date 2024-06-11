namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class UserRequestModel
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public int? RoleId { get; set; }

        public string? Fullname { get; set; }

        public string Email { get; set; } = null!;

        public string? Avatar { get; set; }

        public string? PhoneNumber { get; set; }

        public string? MainAddress { get; set;}

    }
}

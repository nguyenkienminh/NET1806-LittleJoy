namespace NET1806_LittleJoy.API.ViewModels.ResponeModels
{
    public class UserResponseModel
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public int? RoleId { get; set; }

        public string? Fullname { get; set; }

        public string Email { get; set; } = null!;

        public string? Avatar { get; set; }

        public string? PhoneNumber { get; set; }

        public bool? Status { get; set; }

        public int? Points { get; set; }

        public string? UnsignName { get; set; }

        public bool ConfirmEmail { get; set; }
    }
}

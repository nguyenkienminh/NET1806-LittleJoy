namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class UpdateUserRoleRequestModel
    {
        public int Id { get; set; }

        public string? Fullname { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Avatar { get; set; }
    }
}

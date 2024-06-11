namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class UpdateUserRequestModel
    {
        public int Id { get; set; }

        public string? Fullname { get; set; }

        public string? PhoneNumber { get; set; }

        public bool? Status { get; set; }

        public string? Avatar { get; set; }

        public string? MainAddress { get; set; }

        public int? RoleId { get; set; }
    }
}

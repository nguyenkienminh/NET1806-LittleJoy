namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class UpdatePasswordUserRequestModel
    {
        public int Id { get; set; }

        public string? OldPassword { get; set; }

        public string? NewPassword { get; set; }

        public string? ConfirmPassword { get; set; }
    }
}

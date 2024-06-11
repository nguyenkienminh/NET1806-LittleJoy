namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class AddressUpdateRequestModel
    {
        public int Id { get; set; }

        public string? NewAddress { get; set; }

        public bool? IsMainAddress { get; set; }
    }
}

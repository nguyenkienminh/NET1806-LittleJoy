namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class FeedBackRequestModel
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public string? Comment { get; set; }

        public int? Rating { get; set; }

    }

    public class FeedBackUpdateRequestModel
    {
        public int Id { get; set; }

        public string? Comment { get; set; }

        public int? Rating { get; set; }

    }
}

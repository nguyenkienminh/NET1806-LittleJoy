namespace NET1806_LittleJoy.API.ViewModels.ResponeModels
{
    public class FeedBackResponseModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public int ProductId { get; set; }

        public string? Comment { get; set; }

        public int? Rating { get; set; }

        public DateTime? Date { get; set; }
    }
}

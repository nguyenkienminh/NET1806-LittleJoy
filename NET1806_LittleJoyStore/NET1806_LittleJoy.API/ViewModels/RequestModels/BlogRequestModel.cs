namespace NET1806_LittleJoy.API.ViewModels.RequestModels
{
    public class BlogRequestModel
    {
        public int UserId { get; set; }

        public string? Title { get; set; }

        public string? Banner { get; set; }

        public string? Content { get; set; }
    }

    public class BlogRequestUpdateModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? Title { get; set; }

        public string? Banner { get; set; }

        public string? Content { get; set; }
    }
}

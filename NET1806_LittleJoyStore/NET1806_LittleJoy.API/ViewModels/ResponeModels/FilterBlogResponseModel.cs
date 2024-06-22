namespace NET1806_LittleJoy.API.ViewModels.ResponeModels
{
    public class FilterBlogResponseModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? Title { get; set; }

        public string? Banner { get; set; }

        public string? Content { get; set; }

        public DateTime? Date { get; set; }

        public string? UnsignTitle { get; set; }
    }
}

namespace CheckidayViewer.Shared.Models
{
    public class CheckidayHoliday
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public CheckidayHolidayDescription? Description { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Small_Image { get; set; } = string.Empty;
        public string Large_Image { get; set; } = string.Empty;
        public bool Adult { get; set; }
    }

    public class CheckidayHolidayDescription
    {
        public string? Text { get; set; }
        public string? Html { get; set; }
    }
}

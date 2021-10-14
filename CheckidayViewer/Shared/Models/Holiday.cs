namespace CheckidayViewer.Shared.Models
{
    public class Holiday
    {
        public Holiday(string name, string? description, string url, string imageUrl, string smallImageUrl)
        {
            Name = name;
            Description = description;
            Url = url;
            ImageUrl = imageUrl;
            SmallImageUrl = smallImageUrl;
        }

        public string Name { get; set; }
        public string? Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
    }
}

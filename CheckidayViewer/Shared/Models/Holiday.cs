using System.Collections.Generic;

namespace CheckidayViewer.Shared.Models
{
    public class Holiday
    {
        public Holiday(string name, string url, string imageUrl)
        {
            Name = name;
            Url = url;
            ImageUrl = imageUrl;
        }

        public string Name { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }
}

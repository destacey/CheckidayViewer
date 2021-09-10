using System;

namespace CheckidayViewer.Shared.Models
{
    public class CheckidaySettingsOptions
    {
        public const string CheckidaySettings = "CheckidaySettings";

        public string BaseUrl { get; set; } = string.Empty;
        public string DailyApiEndpoint { get; set; } = string.Empty;

        public string TodaysApiEndpoint
            =>  $"{BaseUrl}{DailyApiEndpoint}{DateTime.Now:MM/dd/yyyy}";
    }
}

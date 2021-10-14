using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace CheckidayViewer.Shared.Models
{
    public class CheckidayEventsApiResult
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Date { get; set; }
        public List<CheckidayHoliday> Events { get; set; } = new List<CheckidayHoliday>();
    }
}

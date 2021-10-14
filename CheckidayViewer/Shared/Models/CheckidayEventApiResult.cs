using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace CheckidayViewer.Shared.Models
{
    public class CheckidayEventApiResult
    {
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Last_Update { get; set; }
        public CheckidayHoliday? Event { get; set; }
    }
}

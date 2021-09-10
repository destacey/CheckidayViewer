using System;
using System.Collections.Generic;

namespace CheckidayViewer.Shared.Models
{
    public class CheckidayApiResult
    {
        public string Error { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Number { get; set; } = 0;
        public List<CheckidayHoliday> Holidays { get; set; } = new List<CheckidayHoliday>();
    }
}

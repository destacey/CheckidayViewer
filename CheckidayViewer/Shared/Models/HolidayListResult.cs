using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckidayViewer.Shared.Models
{
    public class HolidayListResult
    {
        public HolidayListResult(DateTime date, List<Holiday> holidays)
        {
            Date = date.Date;
            Count = holidays?.Count ?? 0;
            Holidays = holidays?.ToList() ?? new List<Holiday>();
        }

        public DateTime Date { get; }
        public int Count { get; }
        public List<Holiday> Holidays { get; }
    }
}

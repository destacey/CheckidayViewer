using CheckidayViewer.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CheckidayViewer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly CheckidaySettingsOptions _checkidaySettingsOptions;

        // short term hack
        private static HolidayListResult? _cache;

        public HolidaysController(IOptions<CheckidaySettingsOptions> checkidaySettingsOptions)
        {
            _checkidaySettingsOptions = checkidaySettingsOptions.Value;
        }

        // GET: api/holidays/2021-09-08
        [HttpGet("{date}")]
        public async Task<ActionResult<HolidayListResult>> GetHolidays(DateTime date)
        {
            if (_cache != null)
                return Ok(_cache);

            var holidays = new List<Holiday>();

            using (var client = new HttpClient())
            {
                var dateString = date.ToString("MM/dd/yyyy");
                var uri = $"{_checkidaySettingsOptions.BaseUrl}{_checkidaySettingsOptions.DailyApiEndpoint}{dateString}";

                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                
                var contentString = await response.Content.ReadAsStringAsync();
                var checkidayApiResult = JsonConvert.DeserializeObject<CheckidayEventsApiResult>(contentString);

                if (checkidayApiResult != null)
                {
                    foreach (var checkidayHoliday in checkidayApiResult.Events)
                    {
                        var holiday = await EnrichHolidayDetails(checkidayHoliday);
                        if (holiday != null)
                            holidays.Add(holiday);
                    }
                }
            }

            _cache = new HolidayListResult(date, holidays);
            return Ok(_cache);
        }

        private async Task<Holiday?> EnrichHolidayDetails(CheckidayHoliday checkidayHoliday)
        {
            try
            {
                using var client = new HttpClient();

                var uri = _checkidaySettingsOptions.GetHolidayApiEndpoint(checkidayHoliday.Id);
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                var contentString = await response.Content.ReadAsStringAsync();
                var holidayApiResult = JsonConvert.DeserializeObject<CheckidayEventApiResult>(contentString);

                if (holidayApiResult?.Event is null) return null;

                return new Holiday(holidayApiResult.Event.Name,
                    holidayApiResult.Event.Description?.Html,
                    holidayApiResult.Event.Url,
                    holidayApiResult.Event.Image,
                    holidayApiResult.Event.Small_Image);
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                return null;   
            }

        }
    }
}
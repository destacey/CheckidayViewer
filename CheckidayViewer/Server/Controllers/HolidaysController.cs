using CheckidayViewer.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
                //var uri = $"https://www.checkiday.com/api/3/?d={dateString}";
                var uri = $"{_checkidaySettingsOptions.BaseUrl}{_checkidaySettingsOptions.DailyApiEndpoint}{dateString}";

                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                
                var contentString = await response.Content.ReadAsStringAsync();
                var checkidayApiResult = JsonConvert.DeserializeObject<CheckidayApiResult>(contentString);

                foreach (var checkidayHoliday in checkidayApiResult.Holidays)
                {
                    var holiday = await EnrichHolidayDetails(checkidayHoliday);
                    if (holiday != null)
                        holidays.Add(holiday);
                }
            }

            _cache = new HolidayListResult(date, holidays);
            return Ok(_cache);
        }

        private static async Task<Holiday?> EnrichHolidayDetails(CheckidayHoliday checkidayHoliday)
        {
            try
            {
                var web = new HtmlAgilityPack.HtmlWeb();
                var doc = await web.LoadFromWebAsync(checkidayHoliday.Url);
                var contentSections = doc.DocumentNode
                    .SelectSingleNode("//div[@class='page-content']")?
                    .Descendants("section")
                    .ToList();

                if (contentSections?.Count < 2)
                    return null;

                var imageSrc = contentSections[0].SelectSingleNode("//picture/img").GetAttributeValue("src", "");

                if (imageSrc == null)
                    return null;

                // TODO add Description, Observances

                return new Holiday(checkidayHoliday.Name, checkidayHoliday.Url, imageSrc);
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                return null;   
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Newtonsoft.Json;
using ZenDeskTicker.ExternalModels;
using ZenDeskTicker.Models;

namespace ZenDeskTicker.RequestHandlers
{
    public class GetDaysSinceLastSev
    {
        private readonly IConfiguration _config;

        public GetDaysSinceLastSev(IConfiguration config)
        {
            _config = config;
        }

        public async Task<DaysSinceLastSevResponse> HandleAsync(DaysSinceLastSevRequest request)
        {
            var client = new RestClient("https://panviva.zendesk.com");
            var restRequest = new RestRequest("api/v2/search", Method.GET, DataFormat.Json);
            restRequest.AddQueryParameter("query", $"fieldvalue:severity_{request.SeverityLevel}", true);
            restRequest.AddQueryParameter("sort_by", "created_at", true);
            restRequest.AddQueryParameter("sort_order", "desc", true);
            
            var encodedAuth = Convert.ToBase64String(
                System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes($"{_config["zendeskUsername"]}:{_config["zendeskPassword"]}"));
            restRequest.AddHeader("Authorization", $"Basic {encodedAuth}");

            var result = await client.GetAsync<ZendeskSearch>(restRequest);
            var recentResult = result.results?.FirstOrDefault();
            if (recentResult == null)
            {
                return new DaysSinceLastSevResponse()
                {
                    SeverityLevel = request.SeverityLevel,
                    DaysSinceSev = -1
                };
            }

            var days = (DateTime.UtcNow.Date - recentResult.created_at.Date).TotalDays;
            return new DaysSinceLastSevResponse()
            {
                SeverityLevel = request.SeverityLevel,
                DaysSinceSev = (int)days
            };
        }
    }
}

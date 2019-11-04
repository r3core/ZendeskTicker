using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using RestSharp;
using ZenDeskTicker.ExternalModels;
using ZenDeskTicker.Models;

namespace ZenDeskTicker.RequestHandlers
{
    public class GetDaysSinceUsingAudit
    {
        private readonly IConfiguration _config;

        public GetDaysSinceUsingAudit(IConfiguration config)
        {
            _config = config;
        }

        public async Task<DaysSinceUsingAuditResponse> HandleAsync(DaysSinceUsingAuditRequest request)
        {
            var client = new RestClient("https://panviva.zendesk.com");
            var restRequest = new RestRequest($"api/v2/tickets/{request.TicketId}/audits.json", Method.GET, DataFormat.Json);
            var encodedAuth = Convert.ToBase64String(
                System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes($"{_config["zendeskUsername"]}:{_config["zendeskPassword"]}"));
            restRequest.AddHeader("Authorization", $"Basic {encodedAuth}");
            var result = await client.GetAsync<ZendeskTicketAudit>(restRequest);

            var orderedAudits = result.audits?.OrderByDescending(x => x?.created_at).ToList();
            for (var i = 0; i < orderedAudits.Count; i++)
            {
                var tagsEvent = orderedAudits[i].events.FirstOrDefault(x => x.field_name == "tags");
                if (tagsEvent == null)
                {
                    continue;
                }

                try
                {
                    var castArray = (JsonArray)tagsEvent.value;
                    var tags = castArray.Select(x => (string)x).ToList();
                    if (tags.Select(x => x.ToLower()).Contains($"severity_{request.SeverityLevel}"))
                    {
                        return new DaysSinceUsingAuditResponse()
                        {
                            DaysSinceSev = (DateTime.Now.Date - orderedAudits[i].created_at.Date).TotalDays
                        };
                    }
                }
                catch (Exception)
                {
                }
            }

            return null;
        }
    }
}

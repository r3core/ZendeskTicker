using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using ZenDeskTicker.ExternalModels;
using ZenDeskTicker.Models;
using ZenDeskTicker.Storage;

namespace ZenDeskTicker.RequestHandlers
{
    public class GetDaysSinceLastSev
    {
        private readonly IConfiguration _config;
        private readonly IScoreService _scoreService;

        public GetDaysSinceLastSev(IConfiguration config, IScoreService scoreService)
        {
            _config = config;
            _scoreService = scoreService;
        }

        public async Task<DaysSinceLastSevResponse> HandleAsync(DaysSinceLastSevRequest request)
        {
            var client = new RestClient("https://panviva.zendesk.com");
            var restRequest = new RestRequest("api/v2/search", Method.GET, DataFormat.Json);
            restRequest.AddQueryParameter("query", $"tags:severity_{request.SeverityLevel}", true);
            restRequest.AddQueryParameter("sort_by", "created_at", true);
            restRequest.AddQueryParameter("sort_order", "desc", true);
            
            var encodedAuth = Convert.ToBase64String(
                System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes($"{_config["zendeskUsername"]}:{_config["zendeskPassword"]}"));
            restRequest.AddHeader("Authorization", $"Basic {encodedAuth}");

            var result = await client.GetAsync<ZendeskSearch>(restRequest);

            var useTagDate = bool.Parse(_config["useTagDate"]);
            if (!useTagDate)
            {
                var latestTicket = result.results.Take(1).FirstOrDefault();
                int totalDays = (int)(DateTime.Now.Date - latestTicket.created_at.Date).TotalDays;
                return new DaysSinceLastSevResponse()
                {
                    SeverityLevel = request.SeverityLevel,
                    DaysSinceSev = totalDays,
                    CurrentHighScore = await _scoreService.GetHighScoreAsync(totalDays),
                    TicketCreatedAt = latestTicket.created_at,
                    TicketTitle = latestTicket.raw_subject,
                    Status = latestTicket.status,
                    InvestigativeSteps = latestTicket.custom_fields?.FirstOrDefault(x => x.id.ToString() == _config["zendeskInvestigativeStepsId"])?.value?.ToString(),
                    ResolutionSummary = latestTicket.custom_fields?.FirstOrDefault(x => x.id.ToString() == _config["zendeskResolutionSummaryId"])?.value?.ToString(),
                    TicketSummary = latestTicket.custom_fields?.FirstOrDefault(x => x.id.ToString() == _config["zendeskTicketSummaryId"])?.value?.ToString()
                };
            }

            var topResults = result.results?.Take(int.Parse(_config["auditScanLimit"]));
            if (topResults == null)
            {
                return new DaysSinceLastSevResponse()
                {
                    SeverityLevel = request.SeverityLevel,
                    DaysSinceSev = -1,
                    TicketTitle = string.Empty,
                    TicketCreatedAt = null,
                    InvestigativeSteps = string.Empty,
                    TicketSummary = string.Empty,
                    ResolutionSummary = string.Empty,
                    Status = string.Empty
                };
            }
            var auditDays = new GetDaysSinceUsingAudit(_config);
            var mostRecent = default(ZendeskSearchResult);
            var mostRecentAudit = default(DaysSinceUsingAuditResponse);
            foreach (var nextResult in topResults)
            {
                var auditResult = await auditDays.HandleAsync(new DaysSinceUsingAuditRequest
                {
                    SeverityLevel = request.SeverityLevel,
                    TicketId = nextResult.id
                });

                if (mostRecent == null)
                {
                    mostRecent = nextResult;
                    mostRecentAudit = auditResult;
                }
                else if (auditResult.DaysSinceSev < mostRecentAudit.DaysSinceSev)
                {
                    mostRecent = nextResult;
                    mostRecentAudit = auditResult;
                }
            }

            if (mostRecent == null)
            {
                return new DaysSinceLastSevResponse()
                {
                    SeverityLevel = request.SeverityLevel,
                    DaysSinceSev = -1,
                    TicketTitle = string.Empty,
                    TicketCreatedAt = null,
                    InvestigativeSteps = string.Empty,
                    TicketSummary = string.Empty,
                    ResolutionSummary = string.Empty,
                    Status = string.Empty
                };
            }

            return new DaysSinceLastSevResponse()
            {
                SeverityLevel = request.SeverityLevel,
                DaysSinceSev = (int)mostRecentAudit.DaysSinceSev,
                CurrentHighScore = (int)mostRecentAudit.DaysSinceSev,
                TicketCreatedAt = mostRecent.created_at,
                TicketTitle = mostRecent.raw_subject,
                Status = mostRecent.status,
                InvestigativeSteps = mostRecent.custom_fields?.FirstOrDefault(x => x.id.ToString() == _config["zendeskInvestigativeStepsId"])?.value?.ToString(),
                ResolutionSummary = mostRecent.custom_fields?.FirstOrDefault(x => x.id.ToString() == _config["zendeskResolutionSummaryId"])?.value?.ToString(),
                TicketSummary = mostRecent.custom_fields?.FirstOrDefault(x => x.id.ToString() == _config["zendeskTicketSummaryId"])?.value?.ToString()
            };
        }
    }
}

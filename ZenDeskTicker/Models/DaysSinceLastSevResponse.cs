using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZenDeskTicker.Models
{
    public class DaysSinceLastSevResponse
    {
        public int DaysSinceSev { get; set; }
        public string Status { get; set; }
        public int SeverityLevel { get; set; }
        public string TicketTitle { get; set; }
        public string TicketSummary { get; set; }
        public string InvestigativeSteps { get; set; }
        public string ResolutionSummary { get; set; }
        public DateTime? TicketCreatedAt { get; set; }
        public int? CurrentHighScore { get; set; }
    }
}

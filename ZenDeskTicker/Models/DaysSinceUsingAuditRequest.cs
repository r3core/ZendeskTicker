using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZenDeskTicker.Models
{
    public class DaysSinceUsingAuditRequest
    {
        public int SeverityLevel { get; set; }
        public long TicketId { get; set; }
    }
}

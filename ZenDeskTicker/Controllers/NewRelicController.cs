using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZenDeskTicker.Models;
using ZenDeskTicker.RequestHandlers;

namespace ZenDeskTicker.Controllers
{
    [Route("api/[controller]")]
    public class NewRelicController : Controller
    {
        private readonly GetDaysSinceLastSev _getDaysSinceLastSev;

        public NewRelicController(GetDaysSinceLastSev getDaysSinceLastSev)
        {
            _getDaysSinceLastSev = getDaysSinceLastSev;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<DaysSinceLastSevResponse>> DaysSinceSev(int severity)
        {
            var response = await _getDaysSinceLastSev.HandleAsync(new DaysSinceLastSevRequest { SeverityLevel = severity });
            return new ActionResult<DaysSinceLastSevResponse>(response);
        }
    }
}

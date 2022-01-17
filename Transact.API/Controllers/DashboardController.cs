using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Transact.Data;
using Transact.Data.Abstractions.Services;
using Transact.Data.Models.Common;

namespace Transact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private IDashboardService _dashboardService = null;

        public DashboardController(IWebHostEnvironment hostingEnvironment, IConfiguration config, AppSettings appSettings, IDashboardService dashboardService) : base(hostingEnvironment, config, appSettings)
        {
            _dashboardService = dashboardService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }

        [HttpGet("WidgetAccess")]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult GetWidgetAccess()
        {
            return Ok(_dashboardService.GetWidgetAccessOfRole(Guid.Parse(HttpContext.User.Claims.Where(x => x.Type == "extension_RoleId").FirstOrDefault().Value.ToString())));
        }
    }
}

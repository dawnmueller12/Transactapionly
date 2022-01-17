using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Transact.Data;
using Transact.Data.Abstractions.Services;
using Transact.Data.Models.Common;
using Transact.Data.ViewModels;

namespace Transact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WidgetController : ControllerBase
    {
        private IWidgetService _widgetService = null;

        public WidgetController(IWebHostEnvironment hostingEnvironment, IConfiguration config, AppSettings appSettings, IWidgetService widgetService) : base(hostingEnvironment, config, appSettings)
        {
            _widgetService = widgetService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<WidgetVM>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(_widgetService.GetAllWidgets());
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(WidgetVM), StatusCodes.Status200OK)]
        public IActionResult GetById(Guid Id)
        {
            return Ok(_widgetService.GetById(Id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult Post([FromBody] WidgetVM vmObj)
        {
            return Ok(_widgetService.AddWidget(vmObj));
        }

        [HttpPut]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult Put([FromBody] WidgetVM vmObj)
        {
            return Ok(_widgetService.UpdateWidget(vmObj));
        }

        [HttpPost("AddRoles")]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult AddRoles([FromBody] WidgetRoleVM vmObj)
        {
            return Ok(_widgetService.AddRoleToWidget(vmObj));
        }

        [HttpDelete("DeleteRoles")]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult RemoveRoles([FromBody] WidgetRoleVM vmObj)
        {
            return Ok(_widgetService.RemoveRoleFromWidget(vmObj));
        }

        [HttpGet("{Id}/roles")]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult GetWidgetAccess(Guid Id)
        {
            return Ok(_widgetService.GetWidgetAccess(Id));
        }
    }
}

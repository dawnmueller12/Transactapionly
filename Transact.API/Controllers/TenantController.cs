﻿using Microsoft.AspNetCore.Authorization;
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
    public class TenantController : ControllerBase
    {
        private ITenantService _tenantService = null;

        public TenantController(IWebHostEnvironment hostingEnvironment, IConfiguration config, AppSettings appSettings, ITenantService tenantService) : base(hostingEnvironment, config, appSettings)
        {
            _tenantService = tenantService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TenantVM>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(_tenantService.GetAllTenants());
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(TenantVM), StatusCodes.Status200OK)]
        public IActionResult GetById(Guid Id)
        {
            return Ok(_tenantService.GetById(Id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult Post([FromBody] TenantVM vmObj)
        {
            return Ok(_tenantService.AddTenant(vmObj));
        }

        [HttpPut]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult Put([FromBody] TenantVM vmObj)
        {
            return Ok(_tenantService.UpdateTenant(vmObj));
        }
    }
}

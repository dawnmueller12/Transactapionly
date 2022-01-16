using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Transact.Data;
using Transact.Data.Abstractions.UnitOfWork;
using Transact.Data.Models.Common;
using Transact.Data.Services;
using Transact.Data.ViewModels;

namespace Transact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private RoleService _roleService = null;

        public RoleController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment,
            IConfiguration config, IMapper mapper, AppSettings appSettings) : base(unitOfWork, hostingEnvironment, config, mapper, appSettings)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _roleService = new RoleService(_unitOfWork, _mapper, _appSettings);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RoleVM>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(_roleService.GetAllRoles());
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(RoleVM), StatusCodes.Status200OK)]
        public IActionResult GetById(Guid Id)
        {
            return Ok(_roleService.GetById(Id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult Post([FromBody] RoleVM vmObj)
        {
            return Ok(_roleService.AddRole(vmObj));
        }

        [HttpPut]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult Put([FromBody] RoleVM vmObj)
        {
            return Ok(_roleService.UpdateRole(vmObj));
        }
    }
}

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
    public class UserController : ControllerBase
    {
        private UserService _userService = null;
       

        public UserController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment,
            IConfiguration config, IMapper mapper, AppSettings appSettings) : base(unitOfWork, hostingEnvironment, config, mapper, appSettings)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _userService = new UserService(_unitOfWork, _mapper, _appSettings);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<UserVM>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            string owner = User.Identity.Name;
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(UserVM), StatusCodes.Status200OK)]
        public IActionResult GetById(Guid Id)
        {
            return Ok(_userService.GetById(Id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult Post([FromBody] UserVM vmObj)
        {
            return Ok(_userService.AddUser(vmObj));
        }

        [HttpPut]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult Put([FromBody] UserVM vmObj)
        {
            return Ok(_userService.UpdateUser(vmObj));
        }

        [HttpGet("{Id}/tenants")]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult GetTenants(Guid Id)
        {
            return Ok(_userService.GetTenants(Id));
        }

        [HttpPost("AddTenants")]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult AddUser([FromBody] UserTenantVM vmObj)
        {
            return Ok(_userService.AddTenantToUser(vmObj));
        }

        [HttpDelete("DeleteTenants")]
        [ProducesResponseType(typeof(CustomResponse), StatusCodes.Status200OK)]
        public IActionResult RemoveUser([FromBody] UserTenantVM vmObj)
        {
            return Ok(_userService.RemoveTenantFromUser(vmObj));
        }
    }
}

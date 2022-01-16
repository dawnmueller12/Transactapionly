using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Transact.Data.Abstractions.UnitOfWork;
using Transact.Data.Models.Common;

namespace Transact.API.Controllers
{
    [ApiController]
    public class ControllerBase: Controller
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IWebHostEnvironment _hostingEnvironment;
        protected readonly IConfiguration _config;
        protected readonly IMapper _mapper;
        protected readonly AppSettings _appSettings;

        public ControllerBase(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment, IConfiguration config,
            IMapper mapper, AppSettings appSettings) : base()
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
            _mapper = mapper;
            _appSettings = appSettings;
        }

        public ControllerBase() : base()
        {

        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Transact.Data.Models.Common;

namespace Transact.API.Controllers
{
    [ApiController]
    public class ControllerBase: Controller
    {
        protected readonly IWebHostEnvironment _hostingEnvironment;
        protected readonly IConfiguration _config;
        protected readonly AppSettings _appSettings;

        public ControllerBase(IWebHostEnvironment hostingEnvironment, IConfiguration config, AppSettings appSettings) : base()
        {
            _hostingEnvironment = hostingEnvironment;
            _config = config;
            _appSettings = appSettings;
        }

        public ControllerBase() : base()
        {

        }
    }
}

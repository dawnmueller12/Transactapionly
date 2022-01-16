using AutoMapper;
using Transact.Data.Abstractions.Services;
using Transact.Data.Abstractions.UnitOfWork;
using Transact.Data.Models.Common;

namespace Transact.Data.Services
{
    public class BaseService : IBaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly AppSettings _appSettings;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, AppSettings appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings;
        }
    }
}

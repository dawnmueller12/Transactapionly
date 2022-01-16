using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Transact.Data.Abstractions.Services;
using Transact.Data.Abstractions.UnitOfWork;
using Transact.Data.Models;
using Transact.Data.Models.Common;
using Transact.Data.ViewModels;

namespace Transact.Data.Services
{
    public class TenantService : BaseService, ITenantService
    {
        public TenantService(IUnitOfWork unitOfWork, IMapper mapper, AppSettings appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public CustomResponse GetAllTenants()
        {
            var _tenantRepo = _unitOfWork.GetRepository<Tenant>();
            CustomResponse response = new CustomResponse();
            response.RESPONSE = _mapper.Map<List<Tenant>, List<TenantVM>>(_tenantRepo.GetAll().ToList());
            response.IS_SUCCESS = true;
            return response;
        }

        public CustomResponse GetById(Guid Id)
        {
            var _tenantRepo = _unitOfWork.GetRepository<Tenant>();
            CustomResponse response = new CustomResponse();
            response.RESPONSE = _mapper.Map<Tenant, TenantVM>(_tenantRepo.FindBy(x => x.Id.ToString() == Id.ToString()).FirstOrDefault());
            if (response.RESPONSE == null)
            {
                response.IS_SUCCESS = false;
                response.MESSAGE = "Tenant is not found in DB";
            }
            else
            {
                response.IS_SUCCESS = true;
            }
            return response;
        }

        public CustomResponse AddTenant(TenantVM vmObj)
        {
            var _tenantRepo = _unitOfWork.GetRepository<Tenant>();
            var tenantDBObj = _mapper.Map<TenantVM, Tenant>(vmObj);
            tenantDBObj.Id = Guid.NewGuid();
            _tenantRepo.Add(tenantDBObj);
            _unitOfWork.Commit();
            CustomResponse response = new CustomResponse();
            response.IS_SUCCESS = true;
            return response;
        }

        public CustomResponse UpdateTenant(TenantVM vmObj)
        {
            var _tenantRepo = _unitOfWork.GetRepository<Tenant>();
            var tenantDBObj = _mapper.Map<TenantVM, Tenant>(vmObj);
            _tenantRepo.Edit(tenantDBObj);
            _unitOfWork.Commit();
            CustomResponse response = new CustomResponse();
            if (tenantDBObj == null)
            {
                response.IS_SUCCESS = false;
                response.MESSAGE = "Tenant is not found in DB";
            }
            else
            {
                response.IS_SUCCESS = true;
                response.MESSAGE = "Successfully updated";
            }
            return response;
        }
    }
}

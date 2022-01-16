using AutoMapper;
using Microsoft.Graph;
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
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, AppSettings appSettings) : base(unitOfWork, mapper, appSettings)
        {

        }
        public CustomResponse GetAllUsers()
        {
            var _userRepo = _unitOfWork.GetRepository<UserAccount>();
            CustomResponse response = new CustomResponse();
            response.RESPONSE = _mapper.Map<List<UserAccount>, List<UserVM>>(_userRepo.GetAll().ToList());
            response.IS_SUCCESS = true;
            return response;
        }

        public CustomResponse GetById(Guid Id)
        {
            var _userRepo = _unitOfWork.GetRepository<UserAccount>();
            CustomResponse response = new CustomResponse();
            response.RESPONSE = _mapper.Map<UserAccount, UserVM>(_userRepo.FindBy(x => x.Id.ToString() == Id.ToString()).FirstOrDefault());
            if (response.RESPONSE == null)
            {
                response.IS_SUCCESS = false;
                response.MESSAGE = "User is not found in DB";
            }
            else
            {
                response.IS_SUCCESS = true;
            }
            return response;
        }

        public CustomResponse AddUser(UserVM vmObj)
        {
            GraphService graphService = new GraphService(_appSettings);
            CustomResponse response = graphService.CreateUser(vmObj);
            if (response.IS_SUCCESS)
            {
                var _userRepo = _unitOfWork.GetRepository<UserAccount>();
                var userAccDBObj = _mapper.Map<UserVM, UserAccount>(vmObj);
                User graphUser = (User)response.RESPONSE;
                userAccDBObj.Id = Guid.Parse(graphUser.Id);
                userAccDBObj.CreatedBy = null;
                _userRepo.Add(userAccDBObj);
                _unitOfWork.Commit();
                response.IS_SUCCESS = true;
                response.MESSAGE = "User created successfully";
            }
            else
            {
                response.RESPONSE = null;
            }
            return response;
        }

        public CustomResponse UpdateUser(UserVM vmObj)
        {
            var _userRepo = _unitOfWork.GetRepository<UserAccount>();
            var userAccDBObj = _mapper.Map<UserVM, UserAccount>(vmObj);
            _userRepo.Edit(userAccDBObj);
            _unitOfWork.Commit();
            CustomResponse response = new CustomResponse();
            if (userAccDBObj == null)
            {
                response.IS_SUCCESS = false;
                response.MESSAGE = "User is not found in DB";
            }
            else
            {
                response.IS_SUCCESS = true;
                response.MESSAGE = "Successfully updated";
            }
            return response;
        }

        public CustomResponse AddTenantToUser(UserTenantVM vmObj)
        {
            CustomResponse response = new CustomResponse();

            var _userAccRepo = _unitOfWork.GetRepository<UserAccount>();
            var userAccDBObj = _userAccRepo.FindBy(x => x.Id.ToString() == vmObj.UserId.ToString()).FirstOrDefault();

            if (userAccDBObj == null)
            {
                response.IS_SUCCESS = false;
                response.MESSAGE = "User is not found";
            }
            else
            {
                bool isAnyTenantNotfound = false;
                foreach (Guid tenantId in vmObj.TenantIds)
                {
                    var _tenantRepo = _unitOfWork.GetRepository<Tenant>();
                    var tenantDBObj = _tenantRepo.FindBy(x => x.Id.ToString() == tenantId.ToString()).FirstOrDefault();
                    if (tenantDBObj == null)
                    {
                        isAnyTenantNotfound = true;
                    }
                }
                if (isAnyTenantNotfound == false)
                {
                    bool isAnyNewTenantAdded = false;
                    foreach (Guid tenantId in vmObj.TenantIds)
                    {
                        TenantAccess tenantAccDBObj = null;
                        var _tenantAccRepo = _unitOfWork.GetRepository<TenantAccess>();

                        tenantAccDBObj = _tenantAccRepo.FindBy(x =>
                            x.UserId.ToString() == vmObj.UserId.ToString() &&
                            x.TenantId.ToString() == tenantId.ToString()).FirstOrDefault();
                        if (tenantAccDBObj == null)  // Add record if User is not associated with the Tenant
                        {
                            tenantAccDBObj = new TenantAccess();
                            tenantAccDBObj.Id = Guid.NewGuid();
                            tenantAccDBObj.UserId = vmObj.UserId;
                            tenantAccDBObj.TenantId = tenantId;
                            tenantAccDBObj.CreatedOn = DateTime.UtcNow;
                            _tenantAccRepo.Add(tenantAccDBObj);
                            isAnyNewTenantAdded = true;
                        }
                        if (isAnyNewTenantAdded)
                        {
                            _unitOfWork.Commit();
                        }
                        response.IS_SUCCESS = true;
                        response.MESSAGE = "Tenant(s) added successfully to user";
                    }
                }
            }
            return response;
        }

        public CustomResponse RemoveTenantFromUser(UserTenantVM vmObj)
        {
            CustomResponse response = new CustomResponse();

            TenantAccess tenantAccDBObj = null;
            var _tenantAccRepo = _unitOfWork.GetRepository<TenantAccess>();
            bool isAnyTenantFound = false;
            foreach (Guid tenantId in vmObj.TenantIds)
            {
                tenantAccDBObj = _tenantAccRepo.FindBy(x =>
                    x.UserId.ToString() == vmObj.UserId.ToString() &&
                    x.TenantId.ToString() == tenantId.ToString()).FirstOrDefault();
                if (tenantAccDBObj != null)
                {
                    _tenantAccRepo.Delete(tenantAccDBObj);
                    isAnyTenantFound = true;
                }
            }
            if (isAnyTenantFound)
            {
                _unitOfWork.Commit();
            }
            response.IS_SUCCESS = true;
            response.MESSAGE = "Tenant(s) removed successfully from user";
            return response;
        }

        public CustomResponse GetTenants(Guid Id)
        {
            CustomResponse response = GetById(Id);
            if (response.IS_SUCCESS)  // User found
            {
                var _tenantAccRepo = _unitOfWork.GetRepository<TenantAccess>();
                var tenantAccessList = _tenantAccRepo.AllIncluding(
                    x => x.UserId.ToString() == Id.ToString(),
                    x => x.Tenant);
                var tenantList = new List<TenantVM>();
                foreach (TenantAccess tenantAccObj in tenantAccessList)
                {
                    tenantList.Add(_mapper.Map<Tenant, TenantVM>(tenantAccObj.Tenant));
                }

                ((UserVM)response.RESPONSE).Tenants = tenantList;
                response.IS_SUCCESS = true;
            }
            return response;
        }
    }
}

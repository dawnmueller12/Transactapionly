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
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, AppSettings appSettingss) : base(unitOfWork, mapper, appSettingss)
        {
        }

        public CustomResponse GetAllRoles()
        {
            var _roleRepo = _unitOfWork.GetRepository<Role>();
            CustomResponse response = new CustomResponse();
            response.RESPONSE = _mapper.Map<List<Role>, List<RoleVM>>(_roleRepo.GetAll().ToList());
            response.IS_SUCCESS = true;
            return response;
        }

        public CustomResponse GetById(Guid Id)
        {
            var _roleRepo = _unitOfWork.GetRepository<Role>();
            CustomResponse response = new CustomResponse();
            response.RESPONSE = _mapper.Map<Role, RoleVM>(_roleRepo.FindBy(x => x.Id.ToString() == Id.ToString()).FirstOrDefault());
            if (response.RESPONSE == null)
            {
                response.IS_SUCCESS = false;
                response.MESSAGE = "Role is not found in DB";
            }
            else
            {
                response.IS_SUCCESS = true;
            }
            return response;
        }

        public CustomResponse AddRole(RoleVM vmObj)
        {
            var _roleRepo = _unitOfWork.GetRepository<Role>();
            var roleDBObj = _mapper.Map<RoleVM, Role>(vmObj);
            roleDBObj.Id = Guid.NewGuid();
            _roleRepo.Add(roleDBObj);
            _unitOfWork.Commit();
            CustomResponse response = new CustomResponse();
            response.IS_SUCCESS = true;
            return response;
        }

        public CustomResponse UpdateRole(RoleVM vmObj)
        {
            var _roleRepo = _unitOfWork.GetRepository<Role>();
            var roleDBObj = _mapper.Map<RoleVM, Role>(vmObj);
            _roleRepo.Edit(roleDBObj);
            _unitOfWork.Commit();
            CustomResponse response = new CustomResponse();
            if (roleDBObj == null)
            {
                response.IS_SUCCESS = false;
                response.MESSAGE = "Role is not found in DB";
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

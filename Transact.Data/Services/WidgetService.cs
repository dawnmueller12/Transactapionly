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
    public class WidgetService : BaseService, IWidgetService
    {
        public WidgetService(IUnitOfWork unitOfWork, IMapper mapper, AppSettings appSettings) : base(unitOfWork, mapper, appSettings)
        {

        }

        public CustomResponse GetAllWidgets()
        {
            var _widgetRepo = _unitOfWork.GetRepository<Widget>();
            CustomResponse response = new CustomResponse();
            response.RESPONSE = _mapper.Map<List<Widget>, List<WidgetVM>>(_widgetRepo.GetAll().ToList());
            response.IS_SUCCESS = true;
            return response;
        }

        public CustomResponse GetById(Guid Id)
        {
            var _widgetRepo = _unitOfWork.GetRepository<Widget>();
            CustomResponse response = new CustomResponse();
            response.RESPONSE = _mapper.Map<Widget, WidgetVM>(_widgetRepo.FindBy(x => x.Id.ToString() == Id.ToString()).FirstOrDefault());
            if (response.RESPONSE == null)
            {
                response.IS_SUCCESS = false;
                response.MESSAGE = "Widget is not found";
            }
            else
            {
                response.IS_SUCCESS = true;
            }
            return response;
        }

        public CustomResponse AddWidget(WidgetVM vmObj)
        {
            var _widgetRepo = _unitOfWork.GetRepository<Widget>();
            var widgetAccDBObj = _mapper.Map<WidgetVM, Widget>(vmObj);
            widgetAccDBObj.Id = Guid.NewGuid();
            _widgetRepo.Add(widgetAccDBObj);
            _unitOfWork.Commit();
            CustomResponse response = new CustomResponse();
            response.IS_SUCCESS = true;
            return response;
        }

        public CustomResponse UpdateWidget(WidgetVM vmObj)
        {
            var _widgetRepo = _unitOfWork.GetRepository<Widget>();
            var userAccDBObj = _mapper.Map<WidgetVM, Widget>(vmObj);
            _widgetRepo.Edit(userAccDBObj);
            _unitOfWork.Commit();
            CustomResponse response = new CustomResponse();
            if (userAccDBObj == null)
            {
                response.IS_SUCCESS = false;
                response.MESSAGE = "Widget is not found";
            }
            else
            {
                response.IS_SUCCESS = true;
                response.MESSAGE = "Successfully updated";
            }
            return response;
        }

        public CustomResponse AddRoleToWidget(WidgetRoleVM vmObj)
        {
            CustomResponse response = new CustomResponse();

            var _widgetRepo = _unitOfWork.GetRepository<Widget>();
            var widgetDBObj = _widgetRepo.FindBy(x => x.Id.ToString() == vmObj.WidgetId.ToString()).FirstOrDefault();

            if (widgetDBObj == null)
            {
                response.IS_SUCCESS = false;
                response.MESSAGE = "Widget is not found";
            }
            else
            {
                bool isAnyRoleNotfound = false;
                foreach (Guid roleId in vmObj.RoleIds)
                {
                    var _roleRepo = _unitOfWork.GetRepository<Role>();
                    var roleDBObj = _roleRepo.FindBy(x => x.Id.ToString() == roleId.ToString()).FirstOrDefault();
                    if (roleDBObj == null)
                    {
                        isAnyRoleNotfound = true;
                    }
                }
                if (isAnyRoleNotfound == false)
                {
                    bool isAnyNewRoleAdded = false;
                    foreach (Guid roleId in vmObj.RoleIds)
                    {
                        WidgetAccess widgetAccDBObj = null;
                        var _widgetAccRepo = _unitOfWork.GetRepository<WidgetAccess>();

                        widgetAccDBObj = _widgetAccRepo.FindBy(x =>
                            x.WidgetId.ToString() == vmObj.WidgetId.ToString() &&
                            x.RoleId.ToString() == roleId.ToString()).FirstOrDefault();
                        if (widgetAccDBObj == null)  // Add record if Role is not associated with the Widget
                        {
                            widgetAccDBObj = new WidgetAccess();
                            widgetAccDBObj.Id = Guid.NewGuid();
                            widgetAccDBObj.RoleId = roleId;
                            widgetAccDBObj.WidgetId = vmObj.WidgetId;
                            widgetAccDBObj.CreatedOn = DateTime.UtcNow;
                            _widgetAccRepo.Add(widgetAccDBObj);
                            isAnyNewRoleAdded = true;
                        }
                        if (isAnyNewRoleAdded)
                        {
                            _unitOfWork.Commit();
                        }
                        response.IS_SUCCESS = true;
                        response.MESSAGE = "Role(s) added successfully to widget";
                    }
                }
            }
            return response;
        }

        public CustomResponse RemoveRoleFromWidget(WidgetRoleVM vmObj)
        {
            CustomResponse response = new CustomResponse();

            WidgetAccess widgetAccDBObj = null;
            var _widgetAccRepo = _unitOfWork.GetRepository<WidgetAccess>();
            bool isAnyRoleFound = false;
            foreach (Guid roleId in vmObj.RoleIds)
            {
                widgetAccDBObj = _widgetAccRepo.FindBy(x =>
                    x.WidgetId.ToString() == vmObj.WidgetId.ToString() &&
                    x.RoleId.ToString() == roleId.ToString()).FirstOrDefault();
                if (widgetAccDBObj != null)
                {
                    _widgetAccRepo.Delete(widgetAccDBObj);
                    isAnyRoleFound = true;
                }
            }
            if (isAnyRoleFound)
            {
                _unitOfWork.Commit();
            }
            response.IS_SUCCESS = true;
            response.MESSAGE = "Role(s) removed successfully from widget";
            return response;
        }

        public CustomResponse GetWidgetAccess(Guid Id)
        {
            CustomResponse response = GetById(Id);
            if (response.IS_SUCCESS)  // Widget found
            {
                var _widgetAccsRepo = _unitOfWork.GetRepository<WidgetAccess>();
                var widgetAccessList = _widgetAccsRepo.AllIncluding(
                    x => x.WidgetId.ToString() == Id.ToString(),
                    x => x.Role);
                var rolesList = new List<RoleVM>();
                foreach (WidgetAccess wdgtAccObj in widgetAccessList)
                {
                    rolesList.Add(_mapper.Map<Role, RoleVM>(wdgtAccObj.Role));
                }

                ((WidgetVM)response.RESPONSE).Roles = rolesList;
                response.IS_SUCCESS = true;
            }
            return response;
        }
    }
}

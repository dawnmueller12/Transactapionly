using AutoMapper;
using System;
using Transact.Data.Abstractions.Services;
using Transact.Data.Abstractions.UnitOfWork;
using Transact.Data.Models;
using Transact.Data.Models.Common;
using Transact.Data.ViewModels;

namespace Transact.Data.Services
{
    public class DashboardService : BaseService, IDashboardService
    {
        public DashboardService(IUnitOfWork unitOfWork, IMapper mapper, AppSettings appSettings) : base(unitOfWork, mapper, appSettings)
        {

        }

        public CustomResponse GetWidgetAccessOfRole(Guid roleId)
        {
            CustomResponse response = new CustomResponse();
            if (roleId != Guid.Empty)
            {
                WidgetAccessByRoleVM widgetAccessVMObj = new WidgetAccessByRoleVM();
                widgetAccessVMObj.RoleId = roleId;

                var _widgetAccsRepo = _unitOfWork.GetRepository<WidgetAccess>();
                var widgetAccessList = _widgetAccsRepo.AllIncluding(
                    x => x.RoleId.ToString() == roleId.ToString());
                foreach (WidgetAccess wdgtAccObj in widgetAccessList)
                {
                    widgetAccessVMObj.WidgetIds.Add(wdgtAccObj.WidgetId);
                }


                response.RESPONSE = widgetAccessVMObj;
                response.IS_SUCCESS = true;
            }
            else
            {
                response.IS_SUCCESS = false;
                response.MESSAGE = "roleId should not be empty";
            }
            return response;
        }
    }
}

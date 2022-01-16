using System;
using Transact.Data.ViewModels;

namespace Transact.Data.Abstractions.Services
{
    public interface IRoleService : IBaseService
    {
        CustomResponse AddRole(RoleVM vmObj);
        CustomResponse GetAllRoles();
        CustomResponse GetById(Guid Id);
        CustomResponse UpdateRole(RoleVM vmObj);
    }
}
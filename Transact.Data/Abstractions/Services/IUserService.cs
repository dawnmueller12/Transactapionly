using System;
using Transact.Data.ViewModels;

namespace Transact.Data.Abstractions.Services
{
    public interface IUserService : IBaseService
    {
        CustomResponse AddUser(UserVM vmObj);
        CustomResponse AddTenantToUser(UserTenantVM vmObj);
        CustomResponse GetAllUsers();
        CustomResponse GetById(Guid Id);
        CustomResponse UpdateUser(UserVM vmObj);
        CustomResponse RemoveTenantFromUser(UserTenantVM vmObj);
        CustomResponse GetTenants(Guid Id);

    }
}
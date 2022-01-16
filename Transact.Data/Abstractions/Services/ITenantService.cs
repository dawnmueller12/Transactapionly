using System;
using Transact.Data.ViewModels;

namespace Transact.Data.Abstractions.Services
{
    public interface ITenantService : IBaseService
    {
        CustomResponse AddTenant(TenantVM vmObj);
        CustomResponse GetAllTenants();
        CustomResponse GetById(Guid Id);
        CustomResponse UpdateTenant(TenantVM vmObj);
    }
}
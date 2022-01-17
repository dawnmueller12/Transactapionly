using System;

namespace Transact.Data.Abstractions.Services
{
    public interface IDashboardService : IBaseService
    {
        CustomResponse GetWidgetAccessOfRole(Guid roleId);
    }
}
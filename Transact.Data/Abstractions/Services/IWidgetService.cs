using System;
using Transact.Data.ViewModels;

namespace Transact.Data.Abstractions.Services
{
    public interface IWidgetService : IBaseService
    {
        CustomResponse AddWidget(WidgetVM vmObj);
        CustomResponse GetAllWidgets();
        CustomResponse GetById(Guid Id);
        CustomResponse UpdateWidget(WidgetVM vmObj);
        CustomResponse GetWidgetAccess(Guid Id);
    }
}
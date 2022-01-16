using System;
using System.Collections.Generic;

namespace Transact.Data.ViewModels
{
    public class WidgetRoleVM
    {
        public Guid WidgetId { get; set; }
        public IList<Guid> RoleIds { get; set; }
    }
}

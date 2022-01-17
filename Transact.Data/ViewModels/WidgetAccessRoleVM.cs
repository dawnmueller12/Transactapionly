using System;
using System.Collections.Generic;

namespace Transact.Data.ViewModels
{
    public class WidgetAccessByRoleVM
    {
        public Guid RoleId { get; set; }
        public List<Guid> WidgetIds { get; set; }

        public WidgetAccessByRoleVM()
        {
            WidgetIds = new List<Guid>();
        }
    }
}

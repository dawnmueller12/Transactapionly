using System;
using System.Collections.Generic;

namespace Transact.Data.ViewModels
{
    public class WidgetVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<RoleVM> Roles { get; set; }

        public WidgetVM()
        {
            Roles = new List<RoleVM>();
        }
    }
}

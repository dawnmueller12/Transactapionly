using System;
using System.Collections.Generic;

#nullable disable

namespace Transact.Data.Models
{
    public partial class Role
    {
        public Role()
        {
            UserAccounts = new HashSet<UserAccount>();
            WidgetAccesses = new HashSet<WidgetAccess>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserAccount> UserAccounts { get; set; }
        public virtual ICollection<WidgetAccess> WidgetAccesses { get; set; }
    }
}

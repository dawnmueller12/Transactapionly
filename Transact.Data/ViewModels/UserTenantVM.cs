using System;
using System.Collections.Generic;

namespace Transact.Data.ViewModels
{
    public class UserTenantVM
    {
        public Guid UserId { get; set; }
        public List<Guid> TenantIds { get; set; }
    }
}

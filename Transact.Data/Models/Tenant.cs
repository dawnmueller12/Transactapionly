using System;
using System.Collections.Generic;

#nullable disable

namespace Transact.Data.Models
{
    public partial class Tenant
    {
        public Tenant()
        {
            TenantAccesses = new HashSet<TenantAccess>();
        }

        public Guid Id { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }

        public virtual ICollection<TenantAccess> TenantAccesses { get; set; }
    }
}

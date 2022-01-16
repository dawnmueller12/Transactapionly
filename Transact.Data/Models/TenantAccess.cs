using System;
using System.Collections.Generic;

#nullable disable

namespace Transact.Data.Models
{
    public partial class TenantAccess
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }

        public virtual Tenant Tenant { get; set; }
        public virtual UserAccount User { get; set; }
    }
}

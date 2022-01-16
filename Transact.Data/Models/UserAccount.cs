using System;
using System.Collections.Generic;

#nullable disable

namespace Transact.Data.Models
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            InverseCreatedByNavigation = new HashSet<UserAccount>();
            TenantAccesses = new HashSet<TenantAccess>();
        }

        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? RoleId { get; set; }
        public string Email { get; set; }

        public virtual UserAccount CreatedByNavigation { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<UserAccount> InverseCreatedByNavigation { get; set; }
        public virtual ICollection<TenantAccess> TenantAccesses { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Transact.Data.ViewModels
{
    public class UserVM
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? RoleId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<TenantVM> Tenants { get; set; }
    }
}

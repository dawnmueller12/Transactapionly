using System;

namespace Transact.Data.ViewModels
{
    public class TenantVM
    {
        public Guid Id { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
    }
}

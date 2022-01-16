using System;

#nullable disable

namespace Transact.Data.Models
{
    public partial class WidgetAccess
    {
        public Guid Id { get; set; }
        public Guid WidgetId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual Role Role { get; set; }
        public virtual Widget Widget { get; set; }
    }
}

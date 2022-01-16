using System;
using System.Collections.Generic;

#nullable disable

namespace Transact.Data.Models
{
    public partial class Widget
    {
        public Widget()
        {
            WidgetAccesses = new HashSet<WidgetAccess>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual ICollection<WidgetAccess> WidgetAccesses { get; set; }
    }
}

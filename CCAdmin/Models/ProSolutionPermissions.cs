using System;
using System.Collections.Generic;

namespace CCAdmin.Models
{
    public partial class ProSolutionPermissions
    {
        public string ObjectName { get; set; }
        public string ActionName { get; set; }
        public string ObjectType { get; set; }
        public Guid DataSourceId { get; set; }
        public string UserName { get; set; }
        public bool? IsAllowed { get; set; }
        public Guid PermissionObjectActionId { get; set; }
        public string ObjectCaption { get; set; }
        public Guid ScreenId { get; set; }
    }
}

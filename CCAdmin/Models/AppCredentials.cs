using System;
using System.Collections.Generic;

namespace CCAdmin.Models
{
    public partial class AppCredentials
    {
        public int AppCredentialId { get; set; }
        public string ActivationWord { get; set; }
        public string IdentityKey { get; set; }
        public string ProtectionString { get; set; }
        public DateTime? Expiry { get; set; }
        public string SystemRef { get; set; }
    }
}

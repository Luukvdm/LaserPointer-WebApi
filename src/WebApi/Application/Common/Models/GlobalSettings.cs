using System.Collections.Generic;

namespace LaserPointer.WebApi.Application.Common.Models
{
    public class GlobalSettings
    {
        public virtual string AesSecret { get; set; }
        public virtual string SiteName { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string BasePath { get; set; }
        public virtual string IdentityAuthority { get; set; }
        public virtual string IdentitySecret { get; set; }
        public virtual IList<string> IdentityRequiredPolicies { get; set; }
        public virtual IDictionary<string, string> IdentityScopes { get; set; }
    }
}

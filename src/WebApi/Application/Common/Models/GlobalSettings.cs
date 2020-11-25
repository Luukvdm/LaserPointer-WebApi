namespace LaserPointer.WebApi.Application.Common.Models
{
    public class GlobalSettings
    {
        public virtual string SiteName { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string BasePath { get; set; }
        public virtual string IdentityAuthority { get; set; }
        public virtual string IdentitySecret { get; set; }
    }
}

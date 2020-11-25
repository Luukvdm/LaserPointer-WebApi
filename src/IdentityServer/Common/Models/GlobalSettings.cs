namespace LaserPointer.IdentityServer.Common.Models
{
    public class GlobalSettings
    {
        public virtual string SiteName { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string BasePath { get; set; }
        public virtual string IdentitySecret { get; set; }
    }
}

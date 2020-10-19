namespace LaserPointer.WebApi.Application.Common.Models
{
    public class GlobalSettings
    {
        public virtual string SiteName { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual BaseServiceUriSettings BaseServiceUri { get; set; } = new BaseServiceUriSettings();
        public virtual string BasePath { get; set; }
        public virtual string IdentitySecret { get; set; }

        public class BaseServiceUriSettings
        {
            public string ClientApp { get; set; }
            public string WebApi { get; set; }
            public string Identity { get; set; }
        }
    }
}

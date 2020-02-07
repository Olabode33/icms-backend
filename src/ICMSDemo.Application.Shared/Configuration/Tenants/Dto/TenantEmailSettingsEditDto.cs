using Abp.Auditing;
using ICMSDemo.Configuration.Dto;

namespace ICMSDemo.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}
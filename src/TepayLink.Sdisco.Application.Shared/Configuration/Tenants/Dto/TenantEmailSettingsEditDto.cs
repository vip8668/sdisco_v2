using Abp.Auditing;
using TepayLink.Sdisco.Configuration.Dto;

namespace TepayLink.Sdisco.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}
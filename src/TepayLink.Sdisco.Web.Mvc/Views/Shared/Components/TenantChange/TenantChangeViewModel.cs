using Abp.AutoMapper;
using TepayLink.Sdisco.Sessions.Dto;

namespace TepayLink.Sdisco.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}
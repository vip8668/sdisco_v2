using Abp.AutoMapper;
using TepayLink.Sdisco.MultiTenancy.Dto;

namespace TepayLink.Sdisco.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}
using Abp.AutoMapper;
using TepayLink.Sdisco.MultiTenancy;
using TepayLink.Sdisco.MultiTenancy.Dto;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Common;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}
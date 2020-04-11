using Abp.AutoMapper;
using TepayLink.Sdisco.MultiTenancy.Dto;

namespace TepayLink.Sdisco.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}

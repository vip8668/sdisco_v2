using Abp.Domain.Services;

namespace TepayLink.Sdisco
{
    public abstract class SdiscoDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected SdiscoDomainServiceBase()
        {
            LocalizationSourceName = SdiscoConsts.LocalizationSourceName;
        }
    }
}

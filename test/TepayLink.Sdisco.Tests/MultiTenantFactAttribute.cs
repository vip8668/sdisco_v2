using Xunit;

namespace TepayLink.Sdisco.Tests
{
    public sealed class MultiTenantFactAttribute : FactAttribute
    {
        private readonly bool _multiTenancyEnabled = SdiscoConsts.MultiTenancyEnabled;

        public MultiTenantFactAttribute()
        {
            if (!_multiTenancyEnabled)
            {
                Skip = "MultiTenancy is disabled.";
            }
        }
    }
}

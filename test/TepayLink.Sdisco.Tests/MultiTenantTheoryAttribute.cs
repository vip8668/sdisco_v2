using Xunit;

namespace TepayLink.Sdisco.Tests
{
    public sealed class MultiTenantTheoryAttribute : TheoryAttribute
    {
        private readonly bool _multiTenancyEnabled = SdiscoConsts.MultiTenancyEnabled;
      
        public MultiTenantTheoryAttribute()
        {
            if (!_multiTenancyEnabled)
            {
                Skip = "MultiTenancy is disabled.";
            }
        }
    }
}
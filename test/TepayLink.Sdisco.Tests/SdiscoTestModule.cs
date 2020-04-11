using Abp.Modules;
using TepayLink.Sdisco.Test.Base;

namespace TepayLink.Sdisco.Tests
{
    [DependsOn(typeof(SdiscoTestBaseModule))]
    public class SdiscoTestModule : AbpModule
    {
       
    }
}

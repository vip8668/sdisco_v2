using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TepayLink.Sdisco
{
    [DependsOn(typeof(SdiscoCoreSharedModule))]
    public class SdiscoApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SdiscoApplicationSharedModule).GetAssembly());
        }
    }
}
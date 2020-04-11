using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TepayLink.Sdisco
{
    [DependsOn(typeof(SdiscoXamarinSharedModule))]
    public class SdiscoXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SdiscoXamarinIosModule).GetAssembly());
        }
    }
}
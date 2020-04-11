using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TepayLink.Sdisco
{
    public class SdiscoCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SdiscoCoreSharedModule).GetAssembly());
        }
    }
}
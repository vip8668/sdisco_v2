using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TepayLink.Sdisco
{
    public class SdiscoClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SdiscoClientModule).GetAssembly());
        }
    }
}

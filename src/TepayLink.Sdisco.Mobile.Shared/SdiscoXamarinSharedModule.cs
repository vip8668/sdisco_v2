using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TepayLink.Sdisco
{
    [DependsOn(typeof(SdiscoClientModule), typeof(AbpAutoMapperModule))]
    public class SdiscoXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SdiscoXamarinSharedModule).GetAssembly());
        }
    }
}
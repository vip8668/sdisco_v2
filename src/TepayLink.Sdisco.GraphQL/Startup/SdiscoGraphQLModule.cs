using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TepayLink.Sdisco.Startup
{
    [DependsOn(typeof(SdiscoCoreModule))]
    public class SdiscoGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SdiscoGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}
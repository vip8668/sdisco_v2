using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using TepayLink.Sdisco.Authorization;

namespace TepayLink.Sdisco
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(SdiscoApplicationSharedModule),
        typeof(SdiscoCoreModule)
        )]
    public class SdiscoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SdiscoApplicationModule).GetAssembly());
        }
    }
}
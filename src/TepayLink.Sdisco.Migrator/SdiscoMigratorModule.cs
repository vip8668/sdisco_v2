using Abp.AspNetZeroCore;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using TepayLink.Sdisco.Configuration;
using TepayLink.Sdisco.EntityFrameworkCore;
using TepayLink.Sdisco.Migrator.DependencyInjection;

namespace TepayLink.Sdisco.Migrator
{
    [DependsOn(typeof(SdiscoEntityFrameworkCoreModule))]
    public class SdiscoMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public SdiscoMigratorModule(SdiscoEntityFrameworkCoreModule abpZeroTemplateEntityFrameworkCoreModule)
        {
            abpZeroTemplateEntityFrameworkCoreModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(SdiscoMigratorModule).GetAssembly().GetDirectoryPathOrNull(), addUserSecrets: true
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                SdiscoConsts.ConnectionStringName
                );
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SdiscoMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
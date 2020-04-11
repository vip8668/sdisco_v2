using System;
using System.IO;
using Abp;
using Abp.AspNetZeroCore;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Configuration;
using TepayLink.Sdisco.EntityFrameworkCore;
using TepayLink.Sdisco.MultiTenancy;
using TepayLink.Sdisco.Security.Recaptcha;
using TepayLink.Sdisco.Test.Base.DependencyInjection;
using TepayLink.Sdisco.Test.Base.UiCustomization;
using TepayLink.Sdisco.Test.Base.Url;
using TepayLink.Sdisco.Test.Base.Web;
using TepayLink.Sdisco.UiCustomization;
using TepayLink.Sdisco.Url;
using NSubstitute;

namespace TepayLink.Sdisco.Test.Base
{
    [DependsOn(
        typeof(SdiscoApplicationModule),
        typeof(SdiscoEntityFrameworkCoreModule),
        typeof(AbpTestBaseModule))]
    public class SdiscoTestBaseModule : AbpModule
    {
        public SdiscoTestBaseModule(SdiscoEntityFrameworkCoreModule abpZeroTemplateEntityFrameworkCoreModule)
        {
            abpZeroTemplateEntityFrameworkCoreModule.SkipDbContextRegistration = true;
        }

        public override void PreInitialize()
        {
            var configuration = GetConfiguration();

            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;

            //Disable static mapper usage since it breaks unit tests (see https://github.com/aspnetboilerplate/aspnetboilerplate/issues/2052)
            Configuration.Modules.AbpAutoMapper().UseStaticMapper = false;

            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            RegisterFakeService<AbpZeroDbMigrator>();

            IocManager.Register<IAppUrlService, FakeAppUrlService>();
            IocManager.Register<IWebUrlService, FakeWebUrlService>();
            IocManager.Register<IRecaptchaValidator, FakeRecaptchaValidator>();

            Configuration.ReplaceService<IAppConfigurationAccessor, TestAppConfigurationAccessor>();
            Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
            Configuration.ReplaceService<IUiThemeCustomizerFactory, NullUiThemeCustomizerFactory>();

            Configuration.Modules.AspNetZero().LicenseCode = configuration["AbpZeroLicenseCode"];

            //Uncomment below line to write change logs for the entities below:
            Configuration.EntityHistory.IsEnabled = true;
            Configuration.EntityHistory.Selectors.Add("SdiscoEntities", typeof(User), typeof(Tenant));
        }

        public override void Initialize()
        {
            ServiceCollectionRegistrar.Register(IocManager);
        }

        private void RegisterFakeService<TService>()
            where TService : class
        {
            IocManager.IocContainer.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(() => Substitute.For<TService>())
                    .LifestyleSingleton()
            );
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return AppConfigurations.Get(Directory.GetCurrentDirectory(), addUserSecrets: true);
        }
    }
}

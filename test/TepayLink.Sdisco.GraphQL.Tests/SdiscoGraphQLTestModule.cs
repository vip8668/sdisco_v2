using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using TepayLink.Sdisco.Configure;
using TepayLink.Sdisco.Startup;
using TepayLink.Sdisco.Test.Base;

namespace TepayLink.Sdisco.GraphQL.Tests
{
    [DependsOn(
        typeof(SdiscoGraphQLModule),
        typeof(SdiscoTestBaseModule))]
    public class SdiscoGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SdiscoGraphQLTestModule).GetAssembly());
        }
    }
}
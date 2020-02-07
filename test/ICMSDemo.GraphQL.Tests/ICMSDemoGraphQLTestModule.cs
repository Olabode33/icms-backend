using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ICMSDemo.Configure;
using ICMSDemo.Startup;
using ICMSDemo.Test.Base;

namespace ICMSDemo.GraphQL.Tests
{
    [DependsOn(
        typeof(ICMSDemoGraphQLModule),
        typeof(ICMSDemoTestBaseModule))]
    public class ICMSDemoGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ICMSDemoGraphQLTestModule).GetAssembly());
        }
    }
}
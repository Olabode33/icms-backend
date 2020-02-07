using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ICMSDemo.Startup
{
    [DependsOn(typeof(ICMSDemoCoreModule))]
    public class ICMSDemoGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ICMSDemoGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}
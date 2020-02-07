using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ICMSDemo
{
    [DependsOn(typeof(ICMSDemoClientModule), typeof(AbpAutoMapperModule))]
    public class ICMSDemoXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ICMSDemoXamarinSharedModule).GetAssembly());
        }
    }
}
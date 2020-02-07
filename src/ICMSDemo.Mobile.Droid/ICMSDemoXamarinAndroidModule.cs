using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ICMSDemo
{
    [DependsOn(typeof(ICMSDemoXamarinSharedModule))]
    public class ICMSDemoXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ICMSDemoXamarinAndroidModule).GetAssembly());
        }
    }
}
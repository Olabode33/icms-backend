using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ICMSDemo
{
    [DependsOn(typeof(ICMSDemoCoreSharedModule))]
    public class ICMSDemoApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ICMSDemoApplicationSharedModule).GetAssembly());
        }
    }
}
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ICMSDemo
{
    [DependsOn(typeof(ICMSDemoXamarinSharedModule))]
    public class ICMSDemoXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ICMSDemoXamarinIosModule).GetAssembly());
        }
    }
}
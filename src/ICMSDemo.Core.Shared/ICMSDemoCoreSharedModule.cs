using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ICMSDemo
{
    public class ICMSDemoCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ICMSDemoCoreSharedModule).GetAssembly());
        }
    }
}
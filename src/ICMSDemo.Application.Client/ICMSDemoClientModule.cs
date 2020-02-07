using Abp.Modules;
using Abp.Reflection.Extensions;

namespace ICMSDemo
{
    public class ICMSDemoClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ICMSDemoClientModule).GetAssembly());
        }
    }
}

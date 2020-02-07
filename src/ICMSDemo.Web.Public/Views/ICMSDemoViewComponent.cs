using Abp.AspNetCore.Mvc.ViewComponents;

namespace ICMSDemo.Web.Public.Views
{
    public abstract class ICMSDemoViewComponent : AbpViewComponent
    {
        protected ICMSDemoViewComponent()
        {
            LocalizationSourceName = ICMSDemoConsts.LocalizationSourceName;
        }
    }
}
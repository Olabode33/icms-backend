using Abp.AspNetCore.Mvc.Views;

namespace ICMSDemo.Web.Views
{
    public abstract class ICMSDemoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected ICMSDemoRazorPage()
        {
            LocalizationSourceName = ICMSDemoConsts.LocalizationSourceName;
        }
    }
}

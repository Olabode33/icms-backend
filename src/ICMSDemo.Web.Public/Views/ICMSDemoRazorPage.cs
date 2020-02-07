using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace ICMSDemo.Web.Public.Views
{
    public abstract class ICMSDemoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ICMSDemoRazorPage()
        {
            LocalizationSourceName = ICMSDemoConsts.LocalizationSourceName;
        }
    }
}

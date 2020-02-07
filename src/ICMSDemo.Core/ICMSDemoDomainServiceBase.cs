using Abp.Domain.Services;

namespace ICMSDemo
{
    public abstract class ICMSDemoDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected ICMSDemoDomainServiceBase()
        {
            LocalizationSourceName = ICMSDemoConsts.LocalizationSourceName;
        }
    }
}

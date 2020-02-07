using System.Threading.Tasks;
using Abp.Application.Services;
using ICMSDemo.Configuration.Tenants.Dto;

namespace ICMSDemo.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}

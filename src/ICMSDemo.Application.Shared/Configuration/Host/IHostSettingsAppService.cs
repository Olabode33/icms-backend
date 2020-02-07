using System.Threading.Tasks;
using Abp.Application.Services;
using ICMSDemo.Configuration.Host.Dto;

namespace ICMSDemo.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}

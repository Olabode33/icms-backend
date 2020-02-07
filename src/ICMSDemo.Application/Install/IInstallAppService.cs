using System.Threading.Tasks;
using Abp.Application.Services;
using ICMSDemo.Install.Dto;

namespace ICMSDemo.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}
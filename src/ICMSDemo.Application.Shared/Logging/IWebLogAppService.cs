using Abp.Application.Services;
using ICMSDemo.Dto;
using ICMSDemo.Logging.Dto;

namespace ICMSDemo.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}

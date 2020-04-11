using Abp.Application.Services;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Logging.Dto;

namespace TepayLink.Sdisco.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}

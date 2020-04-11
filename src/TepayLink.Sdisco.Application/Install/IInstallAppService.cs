using System.Threading.Tasks;
using Abp.Application.Services;
using TepayLink.Sdisco.Install.Dto;

namespace TepayLink.Sdisco.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}
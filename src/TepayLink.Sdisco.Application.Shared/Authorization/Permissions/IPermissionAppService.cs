using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization.Permissions.Dto;

namespace TepayLink.Sdisco.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}

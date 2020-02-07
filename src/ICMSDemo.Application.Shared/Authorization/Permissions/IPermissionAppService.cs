using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization.Permissions.Dto;

namespace ICMSDemo.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}

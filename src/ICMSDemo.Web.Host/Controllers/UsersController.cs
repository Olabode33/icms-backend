using Abp.AspNetCore.Mvc.Authorization;
using ICMSDemo.Authorization;
using ICMSDemo.Storage;
using Abp.BackgroundJobs;

namespace ICMSDemo.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}
using System.Threading.Tasks;
using Abp.Application.Services;
using ICMSDemo.Sessions.Dto;

namespace ICMSDemo.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}

using System.Threading.Tasks;
using ICMSDemo.Sessions.Dto;

namespace ICMSDemo.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}

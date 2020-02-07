using System.Threading.Tasks;
using Abp.Application.Services;
using ICMSDemo.Editions.Dto;
using ICMSDemo.MultiTenancy.Dto;

namespace ICMSDemo.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}
using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.Risks.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.Risks
{
    public interface IRisksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRiskForViewDto>> GetAll(GetAllRisksInput input);

        Task<GetRiskForViewDto> GetRiskForView(int id);

		Task<GetRiskForEditOutput> GetRiskForEdit(EntityDto input);

		Task<NameValueDto<int>> CreateOrEdit(CreateOrEditRiskDto input);

		Task Delete(EntityDto input);

		
    }
}
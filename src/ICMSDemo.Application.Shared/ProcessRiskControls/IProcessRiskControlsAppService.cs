using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.ProcessRiskControls.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.ProcessRiskControls
{
    public interface IProcessRiskControlsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProcessRiskControlForViewDto>> GetAll(GetAllProcessRiskControlsInput input);

		Task<GetProcessRiskControlForEditOutput> GetProcessRiskControlForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditProcessRiskControlDto input);

		Task Delete(EntityDto input);

		
		Task<PagedResultDto<ProcessRiskControlProcessRiskLookupTableDto>> GetAllProcessRiskForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProcessRiskControlOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProcessRiskControlControlLookupTableDto>> GetAllControlForLookupTable(GetAllForLookupTableInput input);
		
    }
}
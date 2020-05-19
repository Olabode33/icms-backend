using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.ProcessRisks.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.ProcessRisks
{
    public interface IProcessRisksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProcessRiskForViewDto>> GetAll(GetAllProcessRisksInput input);

        Task<GetProcessRiskForViewDto> GetProcessRiskForView(int id);

		Task<GetProcessRiskForEditOutput> GetProcessRiskForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditProcessRiskDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetProcessRisksToExcel(GetAllProcessRisksForExcelInput input);

		
		Task<PagedResultDto<ProcessRiskOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProcessRiskRiskLookupTableDto>> GetAllRiskForLookupTable(GetAllForLookupTableInput input);
		
    }
}
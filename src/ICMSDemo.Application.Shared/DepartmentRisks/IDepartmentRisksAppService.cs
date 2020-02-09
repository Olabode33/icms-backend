using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.DepartmentRisks.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.DepartmentRisks
{
    public interface IDepartmentRisksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDepartmentRiskForViewDto>> GetAll(GetAllDepartmentRisksInput input);

        Task<GetDepartmentRiskForViewDto> GetDepartmentRiskForView(int id);

		Task<GetDepartmentRiskForEditOutput> GetDepartmentRiskForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDepartmentRiskDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDepartmentRisksToExcel(GetAllDepartmentRisksForExcelInput input);

		Task<PagedResultDto<GetDepartmentRiskForViewDto>> GetRiskForDepartment(GetAllDepartmentRisksInput input);


		Task<PagedResultDto<DepartmentRiskDepartmentLookupTableDto>> GetAllDepartmentForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<DepartmentRiskRiskLookupTableDto>> GetAllRiskForLookupTable(GetAllForLookupTableInput input);
		
    }
}
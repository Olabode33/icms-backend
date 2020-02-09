using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.DepartmentRiskControls.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.DepartmentRiskControls
{
    public interface IDepartmentRiskControlsAppService : IApplicationService 
    {
		Task<PagedResultDto<GetDepartmentRiskControlForViewDto>> GetAllForDepartment(GetAllDepartmentRiskControlsInput input);

		Task<PagedResultDto<GetDepartmentRiskControlForViewDto>> GetAll(GetAllDepartmentRiskControlsInput input);

        Task<GetDepartmentRiskControlForViewDto> GetDepartmentRiskControlForView(int id);

		Task<GetDepartmentRiskControlForEditOutput> GetDepartmentRiskControlForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDepartmentRiskControlDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDepartmentRiskControlsToExcel(GetAllDepartmentRiskControlsForExcelInput input);

		
		Task<PagedResultDto<DepartmentRiskControlDepartmentRiskLookupTableDto>> GetAllDepartmentRiskForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<DepartmentRiskControlControlLookupTableDto>> GetAllControlForLookupTable(GetAllForLookupTableInput input);
		
    }
}
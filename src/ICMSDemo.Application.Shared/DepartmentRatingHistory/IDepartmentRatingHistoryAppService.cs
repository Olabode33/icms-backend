using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.DepartmentRatingHistory.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.DepartmentRatingHistory
{
    public interface IDepartmentRatingHistoryAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDepartmentRatingForViewDto>> GetAll(GetAllDepartmentRatingHistoryInput input);

        Task<GetDepartmentRatingForViewDto> GetDepartmentRatingForView(int id);

		Task<GetDepartmentRatingForEditOutput> GetDepartmentRatingForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDepartmentRatingDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDepartmentRatingHistoryToExcel(GetAllDepartmentRatingHistoryForExcelInput input);

		
		Task<PagedResultDto<DepartmentRatingOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<DepartmentRatingRatingLookupTableDto>> GetAllRatingForLookupTable(GetAllForLookupTableInput input);
		
    }
}
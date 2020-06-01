using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.Departments.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.Departments
{
    public interface IDepartmentsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDepartmentForViewDto>> GetAll(GetAllDepartmentsInput input);

        Task<GetDepartmentForViewDto> GetDepartmentForView(int id);

		Task<GetDepartmentForEditOutput> GetDepartmentForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDepartmentDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDepartmentsToExcel(GetAllDepartmentsForExcelInput input);

		Task<PagedResultDto<GetDepartmentForViewDto>> GetAllForRating(GetAllDepartmentsForRatingInput input);
		Task<PagedResultDto<DepartmentUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<DepartmentOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		
    }
}
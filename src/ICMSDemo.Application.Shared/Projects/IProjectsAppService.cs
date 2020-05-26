using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.Projects.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.Projects
{
    public interface IProjectsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProjectForViewDto>> GetAll(GetAllProjectsInput input);

        Task<GetProjectForViewDto> GetProjectForView(int id);

		Task<GetProjectForEditOutput> GetProjectForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditProjectDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetProjectsToExcel(GetAllProjectsForExcelInput input);

		Task Activate(EntityDto input);


		Task<PagedResultDto<ProjectOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<ProjectOrganizationUnitLookupTableDto>> GetAllProcesses(GetAllForLookupTableInput input);

	}
}
using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.WorkingPaperNews.Dtos;

namespace ICMSDemo.WorkingPaperNews
{
    public interface IWorkingPaperNewsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetWorkingPaperNewForViewDto>> GetAll(GetAllWorkingPaperNewsInput input);

		Task<GetWorkingPaperNewForEditOutput> GetWorkingPaperNewForEdit(EntityDto<Guid> input);

		Task CreateOrEdit(CreateOrEditWorkingPaperNewDto input);

		Task Delete(EntityDto<Guid> input);

		Task ApproveWorkPaper(EntityDto<Guid> input);



		Task<PagedResultDto<WorkingPaperNewTestingTemplateLookupTableDto>> GetAllTestingTemplateForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<WorkingPaperNewOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<WorkingPaperNewUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}
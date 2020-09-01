using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.WorkingPaperReviewComments.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.WorkingPaperReviewComments
{
    public interface IWorkingPaperReviewCommentsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetWorkingPaperReviewCommentForViewDto>> GetAll(GetAllWorkingPaperReviewCommentsInput input);

		Task<GetWorkingPaperReviewCommentForEditOutput> GetWorkingPaperReviewCommentForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditWorkingPaperReviewCommentDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetWorkingPaperReviewCommentsToExcel(GetAllWorkingPaperReviewCommentsForExcelInput input);

		
		Task<PagedResultDto<WorkingPaperReviewCommentUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<WorkingPaperReviewCommentWorkingPaperLookupTableDto>> GetAllWorkingPaperForLookupTable(GetAllForLookupTableInput input);
		
    }
}
using System.Collections.Generic;
using ICMSDemo.WorkingPaperReviewComments.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.WorkingPaperReviewComments.Exporting
{
    public interface IWorkingPaperReviewCommentsExcelExporter
    {
        FileDto ExportToFile(List<GetWorkingPaperReviewCommentForViewDto> workingPaperReviewComments);
    }
}
using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.WorkingPaperReviewComments.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.WorkingPaperReviewComments.Exporting
{
    public class WorkingPaperReviewCommentsExcelExporter : EpPlusExcelExporterBase, IWorkingPaperReviewCommentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public WorkingPaperReviewCommentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetWorkingPaperReviewCommentForViewDto> workingPaperReviewComments)
        {
            return CreateExcelPackage(
                "WorkingPaperReviewComments.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("WorkingPaperReviewComments"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("Priority"),
                        L("Status"),
                        L("Severity"),
                        L("ExpectedCompletionDate"),
                        (L("User")) + L("Name"),
                        (L("WorkingPaper")) + L("Code"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, workingPaperReviewComments,
                        _ => _.WorkingPaperReviewComment.Title,
                        _ => _.WorkingPaperReviewComment.Priority,
                        _ => _.WorkingPaperReviewComment.Status,
                        _ => _.WorkingPaperReviewComment.Severity,
                        _ => _timeZoneConverter.Convert(_.WorkingPaperReviewComment.ExpectedCompletionDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName,
                        _ => _.WorkingPaperCode,
                        _ => _.UserName2
                        );

					var expectedCompletionDateColumn = sheet.Column(5);
                    expectedCompletionDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					expectedCompletionDateColumn.AutoFit();
					
					
                });
        }
    }
}

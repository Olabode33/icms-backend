using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.DepartmentRatingHistory.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.DepartmentRatingHistory.Exporting
{
    public class DepartmentRatingHistoryExcelExporter : EpPlusExcelExporterBase, IDepartmentRatingHistoryExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DepartmentRatingHistoryExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDepartmentRatingForViewDto> departmentRatingHistory)
        {
            return CreateExcelPackage(
                "DepartmentRatingHistory.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("DepartmentRatingHistory"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("RatingDate"),
                        L("ChangeType"),
                        (L("OrganizationUnit")) + L("DisplayName"),
                        (L("Rating")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, departmentRatingHistory,
                        _ => _timeZoneConverter.Convert(_.DepartmentRating.RatingDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.DepartmentRating.ChangeType,
                        _ => _.OrganizationUnitDisplayName,
                        _ => _.RatingName
                        );

					var ratingDateColumn = sheet.Column(1);
                    ratingDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					ratingDateColumn.AutoFit();
					
					
                });
        }
    }
}

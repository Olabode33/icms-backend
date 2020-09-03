using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.BusinessObjectives.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.BusinessObjectives.Exporting
{
    public class BusinessObjectivesExcelExporter : EpPlusExcelExporterBase, IBusinessObjectivesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessObjectivesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessObjectiveForViewDto> businessObjectives)
        {
            return CreateExcelPackage(
                "BusinessObjectives.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BusinessObjectives"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("StartDate"),
                        L("EndDate"),
                        L("ObjectiveType"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, businessObjectives,
                        _ => _.BusinessObjective.Name,
                        _ => _timeZoneConverter.Convert(_.BusinessObjective.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.BusinessObjective.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.BusinessObjective.ObjectiveType,
                        _ => _.UserName
                        );

					var startDateColumn = sheet.Column(2);
                    startDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					startDateColumn.AutoFit();
					var endDateColumn = sheet.Column(3);
                    endDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					endDateColumn.AutoFit();
					
					
                });
        }
    }
}

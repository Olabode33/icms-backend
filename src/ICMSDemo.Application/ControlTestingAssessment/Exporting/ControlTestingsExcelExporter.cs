using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.ControlTestingAssessment.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.ControlTestingAssessment.Exporting
{
    public class ControlTestingsExcelExporter : EpPlusExcelExporterBase, IControlTestingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ControlTestingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetControlTestingForViewDto> controlTestings)
        {
            return CreateExcelPackage(
                "ControlTestings.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ControlTestings"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("TestingTemplateId"),
                        L("EndDate")
                        );

                    AddObjects(
                        sheet, 2, controlTestings,
                        _ => _.ControlTesting.Name,
                        _ => _.ControlTesting.TestingTemplateId,
                        _ => _.ControlTesting.EndDate
                        );

					
					
                });
        }
    }
}

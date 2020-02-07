using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.TestingTemplates.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.TestingTemplates.Exporting
{
    public class TestingTemplatesExcelExporter : EpPlusExcelExporterBase, ITestingTemplatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TestingTemplatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTestingTemplateForViewDto> testingTemplates)
        {
            return CreateExcelPackage(
                "TestingTemplates.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("TestingTemplates"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("DetailedInstructions"),
                        L("Title"),
                        L("Frequency"),
                        (L("DepartmentRiskControl")) + L("Code")
                        );

                    AddObjects(
                        sheet, 2, testingTemplates,
                        _ => _.TestingTemplate.Code,
                        _ => _.TestingTemplate.DetailedInstructions,
                        _ => _.TestingTemplate.Title,
                        _ => _.TestingTemplate.Frequency,
                        _ => _.DepartmentRiskControlCode
                        );

					

                });
        }
    }
}

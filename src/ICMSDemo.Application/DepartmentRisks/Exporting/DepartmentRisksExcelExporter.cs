using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.DepartmentRisks.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.DepartmentRisks.Exporting
{
    public class DepartmentRisksExcelExporter : EpPlusExcelExporterBase, IDepartmentRisksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DepartmentRisksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDepartmentRiskForViewDto> departmentRisks)
        {
            return CreateExcelPackage(
                "DepartmentRisks.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("DepartmentRisks"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Comments"),
                        (L("Department")) + L("Name"),
                        (L("Risk")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, departmentRisks,
                        _ => _.DepartmentRisk.Code,
                        _ => _.DepartmentRisk.Comments,
                        _ => _.DepartmentName,
                        _ => _.RiskName
                        );

					

                });
        }
    }
}

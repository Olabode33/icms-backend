using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.DepartmentRiskControls.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.DepartmentRiskControls.Exporting
{
    public class DepartmentRiskControlsExcelExporter : EpPlusExcelExporterBase, IDepartmentRiskControlsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DepartmentRiskControlsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDepartmentRiskControlForViewDto> departmentRiskControls)
        {
            return CreateExcelPackage(
                "DepartmentRiskControls.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("DepartmentRiskControls"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Notes"),
                        L("Frequency"),
                        (L("DepartmentRisk")) + L("Code"),
                        (L("Control")) + L("Code")
                        );

                    AddObjects(
                        sheet, 2, departmentRiskControls,
                        _ => _.DepartmentRiskControl.Code,
                        _ => _.DepartmentRiskControl.Notes,
                        _ => _.DepartmentRiskControl.Frequency,
                        _ => _.DepartmentRiskCode,
                        _ => _.ControlCode
                        );

					

                });
        }
    }
}

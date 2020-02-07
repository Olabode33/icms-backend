using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.Controls.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.Controls.Exporting
{
    public class ControlsExcelExporter : EpPlusExcelExporterBase, IControlsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ControlsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetControlForViewDto> controls)
        {
            return CreateExcelPackage(
                "Controls.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Controls"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("Description"),
                        L("ControlType"),
                        L("Frequency")
                        );

                    AddObjects(
                        sheet, 2, controls,
                        _ => _.Control.Code,
                        _ => _.Control.Name,
                        _ => _.Control.Description,
                        _ => _.Control.ControlType,
                        _ => _.Control.Frequency
                        );

					

                });
        }
    }
}

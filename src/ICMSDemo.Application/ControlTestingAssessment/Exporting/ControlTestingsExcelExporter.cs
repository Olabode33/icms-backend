using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.ControlTestingAssessment.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;
using ICMSDemo.Processes.Dtos;

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

        public FileDto ExportToFile(List<GetProcessForViewDto> controlTestings)
        {
            return CreateExcelPackage(
                "Processes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ControlTestings"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description")
                      
                        );

                    AddObjects(
                        sheet, 2, controlTestings,
                        _ => _.Process.Name,
                        _ => _.Process.Description
                        );

					
					
                });
        }
    }
}

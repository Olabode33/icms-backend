using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.Dto;
using ICMSDemo.Processes.Dtos;
using ICMSDemo.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICMSDemo.Processes.Exporting
{
   
    public class ProcessesExporter : EpPlusExcelExporterBase, IProcessingExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProcessesExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProcessForViewDto> controls)
        {
            return CreateExcelPackage(
                "Processes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Controls"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Casade")
                        );

                    AddObjects(
                        sheet, 2, controls,
                        _ => _.Process.Name,
                        _ => _.Process.Description,
                        _ => _.Process.Casade
                        );



                });
        }
    }
}

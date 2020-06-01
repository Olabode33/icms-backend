using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.LibraryRisks.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.LibraryRisks.Exporting
{
    public class LibraryRisksExcelExporter : EpPlusExcelExporterBase, ILibraryRisksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LibraryRisksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLibraryRiskForViewDto> libraryRisks)
        {
            return CreateExcelPackage(
                "LibraryRisks.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("LibraryRisks"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Process"),
                        L("Description"),
                        L("SubProcess")
                        );

                    AddObjects(
                        sheet, 2, libraryRisks,
                        _ => _.LibraryRisk.Name,
                        _ => _.LibraryRisk.Process,
                        _ => _.LibraryRisk.Description,
                        _ => _.LibraryRisk.SubProcess
                        );

					
					
                });
        }
    }
}

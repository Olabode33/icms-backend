using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.LibraryControls.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.LibraryControls.Exporting
{
    public class LibraryControlsExcelExporter : EpPlusExcelExporterBase, ILibraryControlsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LibraryControlsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLibraryControlForViewDto> libraryControls)
        {
            return CreateExcelPackage(
                "LibraryControls.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("LibraryControls"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Process"),
                        L("SubProcess"),
                        L("Risk"),
                        L("ControlType"),
                        L("ControlPoint"),
                        L("Frequency"),
                        L("InformationProcessingObjectives")
                        );

                    AddObjects(
                        sheet, 2, libraryControls,
                        _ => _.LibraryControl.Name,
                        _ => _.LibraryControl.Description,
                        _ => _.LibraryControl.Process,
                        _ => _.LibraryControl.SubProcess,
                        _ => _.LibraryControl.Risk,
                        _ => _.LibraryControl.ControlType,
                        _ => _.LibraryControl.ControlPoint,
                        _ => _.LibraryControl.Frequency,
                        _ => _.LibraryControl.InformationProcessingObjectives
                        );

					
					
                });
        }
    }
}

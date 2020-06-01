using System.Collections.Generic;
using ICMSDemo.LibraryRisks.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.LibraryRisks.Exporting
{
    public interface ILibraryRisksExcelExporter
    {
        FileDto ExportToFile(List<GetLibraryRiskForViewDto> libraryRisks);
    }
}
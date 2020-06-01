using System.Collections.Generic;
using ICMSDemo.LibraryControls.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.LibraryControls.Exporting
{
    public interface ILibraryControlsExcelExporter
    {
        FileDto ExportToFile(List<GetLibraryControlForViewDto> libraryControls);
    }
}
using System.Collections.Generic;
using ICMSDemo.Controls.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.Controls.Exporting
{
    public interface IControlsExcelExporter
    {
        FileDto ExportToFile(List<GetControlForViewDto> controls);
    }
}
using ICMSDemo.Dto;
using ICMSDemo.Processes.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICMSDemo.Processes.Exporting
{
    
    public interface IProcessingExcelExporter
    {
        FileDto ExportToFile(List<GetProcessForViewDto> processes);
    }
}

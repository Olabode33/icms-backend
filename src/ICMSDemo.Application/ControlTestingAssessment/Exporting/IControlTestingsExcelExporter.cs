using System.Collections.Generic;
using ICMSDemo.ControlTestingAssessment.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Processes.Dtos;

namespace ICMSDemo.ControlTestingAssessment.Exporting
{
    public interface IControlTestingsExcelExporter
    {
        FileDto ExportToFile(List<GetProcessForViewDto> controlTestings);
    }
}
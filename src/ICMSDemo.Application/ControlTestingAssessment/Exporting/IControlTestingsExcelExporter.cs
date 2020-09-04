using System.Collections.Generic;
using ICMSDemo.ControlTestingAssessment.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.ControlTestingAssessment.Exporting
{
    public interface IControlTestingsExcelExporter
    {
        FileDto ExportToFile(List<GetControlTestingForViewDto> controlTestings);
    }
}
using System.Collections.Generic;
using ICMSDemo.ExceptionIncidents.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.ExceptionIncidents.Exporting
{
    public interface IExceptionIncidentsExcelExporter
    {
        FileDto ExportToFile(List<GetExceptionIncidentForViewDto> exceptionIncidents);
    }
}
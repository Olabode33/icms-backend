using System.Collections.Generic;
using ICMSDemo.BusinessObjectives.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.BusinessObjectives.Exporting
{
    public interface IBusinessObjectivesExcelExporter
    {
        FileDto ExportToFile(List<GetBusinessObjectiveForViewDto> businessObjectives);
    }
}
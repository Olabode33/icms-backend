using System.Collections.Generic;
using ICMSDemo.Projects.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.Projects.Exporting
{
    public interface IProjectsExcelExporter
    {
        FileDto ExportToFile(List<GetProjectForViewDto> projects);
    }
}
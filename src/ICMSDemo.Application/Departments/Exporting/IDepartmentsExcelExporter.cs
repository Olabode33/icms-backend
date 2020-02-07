using System.Collections.Generic;
using ICMSDemo.Departments.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.Departments.Exporting
{
    public interface IDepartmentsExcelExporter
    {
        FileDto ExportToFile(List<GetDepartmentForViewDto> departments);
    }
}
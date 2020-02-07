using System.Collections.Generic;
using ICMSDemo.DepartmentRisks.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.DepartmentRisks.Exporting
{
    public interface IDepartmentRisksExcelExporter
    {
        FileDto ExportToFile(List<GetDepartmentRiskForViewDto> departmentRisks);
    }
}
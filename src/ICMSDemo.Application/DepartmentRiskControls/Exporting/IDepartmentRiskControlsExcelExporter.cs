using System.Collections.Generic;
using ICMSDemo.DepartmentRiskControls.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.DepartmentRiskControls.Exporting
{
    public interface IDepartmentRiskControlsExcelExporter
    {
        FileDto ExportToFile(List<GetDepartmentRiskControlForViewDto> departmentRiskControls);
    }
}
using System.Collections.Generic;
using ICMSDemo.DepartmentRatingHistory.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.DepartmentRatingHistory.Exporting
{
    public interface IDepartmentRatingHistoryExcelExporter
    {
        FileDto ExportToFile(List<GetDepartmentRatingForViewDto> departmentRatingHistory);
    }
}
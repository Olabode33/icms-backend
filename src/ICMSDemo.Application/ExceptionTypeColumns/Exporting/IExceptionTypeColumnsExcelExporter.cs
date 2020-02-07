using System.Collections.Generic;
using ICMSDemo.ExceptionTypeColumns.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.ExceptionTypeColumns.Exporting
{
    public interface IExceptionTypeColumnsExcelExporter
    {
        FileDto ExportToFile(List<GetExceptionTypeColumnForViewDto> exceptionTypeColumns);
    }
}
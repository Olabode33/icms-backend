using System.Collections.Generic;
using ICMSDemo.ExceptionTypes.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.ExceptionTypes.Exporting
{
    public interface IExceptionTypesExcelExporter
    {
        FileDto ExportToFile(List<GetExceptionTypeForViewDto> exceptionTypes);
    }
}
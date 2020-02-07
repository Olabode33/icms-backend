using System.Collections.Generic;
using ICMSDemo.DataLists.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.DataLists.Exporting
{
    public interface IDataListsExcelExporter
    {
        FileDto ExportToFile(List<GetDataListForViewDto> dataLists);
    }
}
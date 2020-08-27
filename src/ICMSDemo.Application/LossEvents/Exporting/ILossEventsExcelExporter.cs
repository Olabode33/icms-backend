using System.Collections.Generic;
using ICMSDemo.LossEvents.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.LossEvents.Exporting
{
    public interface ILossEventsExcelExporter
    {
        FileDto ExportToFile(List<GetLossEventForViewDto> lossEvents);
    }
}
using System.Collections.Generic;
using ICMSDemo.ProcessRisks.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.ProcessRisks.Exporting
{
    public interface IProcessRisksExcelExporter
    {
        FileDto ExportToFile(List<GetProcessRiskForViewDto> processRisks);
    }
}
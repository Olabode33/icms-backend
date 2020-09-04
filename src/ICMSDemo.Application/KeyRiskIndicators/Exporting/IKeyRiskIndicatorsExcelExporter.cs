using System.Collections.Generic;
using ICMSDemo.KeyRiskIndicators.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.KeyRiskIndicators.Exporting
{
    public interface IKeyRiskIndicatorsExcelExporter
    {
        FileDto ExportToFile(List<GetKeyRiskIndicatorForViewDto> keyRiskIndicators);
    }
}
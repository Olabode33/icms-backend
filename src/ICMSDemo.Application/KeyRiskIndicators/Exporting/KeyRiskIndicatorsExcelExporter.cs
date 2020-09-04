using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.KeyRiskIndicators.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.KeyRiskIndicators.Exporting
{
    public class KeyRiskIndicatorsExcelExporter : EpPlusExcelExporterBase, IKeyRiskIndicatorsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public KeyRiskIndicatorsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetKeyRiskIndicatorForViewDto> keyRiskIndicators)
        {
            return CreateExcelPackage(
                "KeyRiskIndicators.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("KeyRiskIndicators"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Nature"),
                        L("LowLevel"),
                        (L("ExceptionType")) + L("Code"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, keyRiskIndicators,
                        _ => _.KeyRiskIndicator.Name,
                        _ => _.KeyRiskIndicator.Nature,
                        _ => _.KeyRiskIndicator.LowLevel,
                        _ => _.ExceptionTypeCode,
                        _ => _.UserName
                        );

					
					
                });
        }
    }
}

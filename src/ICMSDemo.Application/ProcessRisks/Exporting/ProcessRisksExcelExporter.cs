using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.ProcessRisks.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.ProcessRisks.Exporting
{
    public class ProcessRisksExcelExporter : EpPlusExcelExporterBase, IProcessRisksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProcessRisksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProcessRiskForViewDto> processRisks)
        {
            return CreateExcelPackage(
                "ProcessRisks.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ProcessRisks"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Comments"),
                        L("Cascade"),
                        (L("OrganizationUnit")) + L("DisplayName"),
                        (L("Risk")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, processRisks,
                        _ => _.ProcessRisk.Code,
                        _ => _.ProcessRisk.Comments,
                        _ => _.ProcessRisk.Cascade,
                        _ => _.ProcessName,
                        _ => _.RiskName
                        );

					
					
                });
        }
    }
}

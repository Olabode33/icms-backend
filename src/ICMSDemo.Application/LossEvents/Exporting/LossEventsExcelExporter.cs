using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.LossEvents.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.LossEvents.Exporting
{
    public class LossEventsExcelExporter : EpPlusExcelExporterBase, ILossEventsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LossEventsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLossEventForViewDto> lossEvents)
        {
            return CreateExcelPackage(
                "LossEvents.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("LossEvents"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Amount"),
                        L("DateOccured"),
                        L("DateDiscovered"),
                        L("LossType"),
                        L("Status"),
                        L("LossCategory"),
                        (L("User")) + L("Name"),
                        (L("OrganizationUnit")) + L("DisplayName")
                        );

                    AddObjects(
                        sheet, 2, lossEvents,
                        _ => _.LossEvent.Amount,
                        _ => _timeZoneConverter.Convert(_.LossEvent.DateOccured, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.LossEvent.DateDiscovered, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LossEvent.LossTypeId,
                        _ => _.LossEvent.Status,
                        _ => _.LossEvent.LossCategory,
                        _ => _.UserName,
                        _ => _.OrganizationUnitDisplayName
                        );

					var dateOccuredColumn = sheet.Column(2);
                    dateOccuredColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					dateOccuredColumn.AutoFit();
					var dateDiscoveredColumn = sheet.Column(3);
                    dateDiscoveredColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					dateDiscoveredColumn.AutoFit();
					
					
                });
        }
    }
}

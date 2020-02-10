using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.ExceptionIncidents.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.ExceptionIncidents.Exporting
{
    public class ExceptionIncidentsExcelExporter : EpPlusExcelExporterBase, IExceptionIncidentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ExceptionIncidentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetExceptionIncidentForViewDto> exceptionIncidents)
        {
            return CreateExcelPackage(
                "ExceptionIncidents.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ExceptionIncidents"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Date"),
                        L("Description"),
                        L("Status"),
                        L("ClosureDate"),
                        L("ClosureComments"),
                        L("RaisedByClosureComments"),
                        (L("ExceptionType")) + L("Name"),
                        (L("User")) + L("Name"),
                        (L("TestingTemplate")) + L("Code"),
                        (L("OrganizationUnit")) + L("DisplayName")
                        );

                    AddObjects(
                        sheet, 2, exceptionIncidents,
                        _ => _.ExceptionIncident.Code,
                        _ => _timeZoneConverter.Convert(_.ExceptionIncident.Date, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ExceptionIncident.Description,
                        _ => _.ExceptionIncident.Status,
                        _ => _timeZoneConverter.Convert(_.ExceptionIncident.ClosureDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ExceptionIncident.ClosureComments,
                        _ => _.ExceptionIncident.RaisedByClosureComments,
                        _ => _.ExceptionTypeName,
                        _ => _.UserName,
                        _ => _.WorkingPaperCode,
                        _ => _.OrganizationUnitDisplayName
                        );

					var dateColumn = sheet.Column(2);
                    dateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					dateColumn.AutoFit();
					var closureDateColumn = sheet.Column(5);
                    closureDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					closureDateColumn.AutoFit();
					

                });
        }
    }
}

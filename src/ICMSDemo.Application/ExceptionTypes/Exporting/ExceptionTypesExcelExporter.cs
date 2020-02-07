using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.ExceptionTypes.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.ExceptionTypes.Exporting
{
    public class ExceptionTypesExcelExporter : EpPlusExcelExporterBase, IExceptionTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ExceptionTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetExceptionTypeForViewDto> exceptionTypes)
        {
            return CreateExcelPackage(
                "ExceptionTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ExceptionTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("Description"),
                        L("Severity"),
                        L("TargetRemediation")
                        );

                    AddObjects(
                        sheet, 2, exceptionTypes,
                        _ => _.ExceptionType.Code,
                        _ => _.ExceptionType.Name,
                        _ => _.ExceptionType.Description,
                        _ => _.ExceptionType.Severity,
                        _ => _.ExceptionType.TargetRemediation
                        );

					

                });
        }
    }
}

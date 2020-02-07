using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.ExceptionTypeColumns.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.ExceptionTypeColumns.Exporting
{
    public class ExceptionTypeColumnsExcelExporter : EpPlusExcelExporterBase, IExceptionTypeColumnsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ExceptionTypeColumnsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetExceptionTypeColumnForViewDto> exceptionTypeColumns)
        {
            return CreateExcelPackage(
                "ExceptionTypeColumns.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ExceptionTypeColumns"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("DataType"),
                        L("Required"),
                        L("Minimum"),
                        L("Maximum"),
                        (L("ExceptionType")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, exceptionTypeColumns,
                        _ => _.ExceptionTypeColumn.Name,
                        _ => _.ExceptionTypeColumn.DataType,
                        _ => _.ExceptionTypeColumn.Required,
                        _ => _.ExceptionTypeColumn.Minimum,
                        _ => _.ExceptionTypeColumn.Maximum,
                        _ => _.ExceptionTypeName
                        );

					

                });
        }
    }
}

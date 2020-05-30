using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.Ratings.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.Ratings.Exporting
{
    public class RatingsExcelExporter : EpPlusExcelExporterBase, IRatingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RatingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRatingForViewDto> ratings)
        {
            return CreateExcelPackage(
                "Ratings.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Ratings"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Code"),
                        L("Description"),
                
                        L("UpperBoundary")
                        );

                    AddObjects(
                        sheet, 2, ratings,
                        _ => _.Rating.Name,
                        _ => _.Rating.Code,
                        _ => _.Rating.Description,

                        _ => _.Rating.UpperBoundary
                        );

					
					
                });
        }
    }
}

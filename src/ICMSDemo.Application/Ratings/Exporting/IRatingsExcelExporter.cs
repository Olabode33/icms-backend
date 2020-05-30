using System.Collections.Generic;
using ICMSDemo.Ratings.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.Ratings.Exporting
{
    public interface IRatingsExcelExporter
    {
        FileDto ExportToFile(List<GetRatingForViewDto> ratings);
    }
}
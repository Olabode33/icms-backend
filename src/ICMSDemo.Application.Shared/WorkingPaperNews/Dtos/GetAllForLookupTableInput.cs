using Abp.Application.Services.Dto;

namespace ICMSDemo.WorkingPaperNews.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
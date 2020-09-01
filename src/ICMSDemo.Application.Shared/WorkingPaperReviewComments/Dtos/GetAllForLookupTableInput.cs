using Abp.Application.Services.Dto;

namespace ICMSDemo.WorkingPaperReviewComments.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
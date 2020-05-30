using Abp.Application.Services.Dto;

namespace ICMSDemo.DepartmentRatingHistory.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
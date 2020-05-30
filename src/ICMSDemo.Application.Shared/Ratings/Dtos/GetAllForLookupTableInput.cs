using Abp.Application.Services.Dto;

namespace ICMSDemo.Ratings.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
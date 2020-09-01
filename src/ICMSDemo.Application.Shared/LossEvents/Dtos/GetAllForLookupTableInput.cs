using Abp.Application.Services.Dto;

namespace ICMSDemo.LossEvents.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
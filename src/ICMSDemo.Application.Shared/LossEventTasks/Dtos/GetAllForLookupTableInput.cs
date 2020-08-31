using Abp.Application.Services.Dto;

namespace ICMSDemo.LossEventTasks.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
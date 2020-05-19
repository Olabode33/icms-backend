using Abp.Application.Services.Dto;

namespace ICMSDemo.Processes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
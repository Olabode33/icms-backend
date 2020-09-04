using Abp.Application.Services.Dto;

namespace ICMSDemo.BusinessObjectives.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
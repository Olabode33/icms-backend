using Abp.Application.Services.Dto;

namespace ICMSDemo.ExceptionIncidents.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
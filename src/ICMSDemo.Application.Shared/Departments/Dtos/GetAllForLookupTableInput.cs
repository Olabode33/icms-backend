using Abp.Application.Services.Dto;

namespace ICMSDemo.Departments.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
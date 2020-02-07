using Abp.Application.Services.Dto;

namespace ICMSDemo.DepartmentRisks.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
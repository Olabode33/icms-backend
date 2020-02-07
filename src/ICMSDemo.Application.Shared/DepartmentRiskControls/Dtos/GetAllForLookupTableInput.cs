using Abp.Application.Services.Dto;

namespace ICMSDemo.DepartmentRiskControls.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
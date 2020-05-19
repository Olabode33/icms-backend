using Abp.Application.Services.Dto;

namespace ICMSDemo.ProcessRiskControls.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
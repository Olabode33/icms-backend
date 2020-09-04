using Abp.Application.Services.Dto;

namespace ICMSDemo.KeyRiskIndicators.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
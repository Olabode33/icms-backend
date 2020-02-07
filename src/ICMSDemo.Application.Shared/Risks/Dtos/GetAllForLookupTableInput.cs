using Abp.Application.Services.Dto;

namespace ICMSDemo.Risks.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
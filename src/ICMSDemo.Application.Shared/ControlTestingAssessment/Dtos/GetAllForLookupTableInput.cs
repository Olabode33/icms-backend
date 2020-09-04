using Abp.Application.Services.Dto;

namespace ICMSDemo.ControlTestingAssessment.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
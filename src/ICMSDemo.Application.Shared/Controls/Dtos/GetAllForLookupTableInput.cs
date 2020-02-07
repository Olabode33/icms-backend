using Abp.Application.Services.Dto;

namespace ICMSDemo.Controls.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
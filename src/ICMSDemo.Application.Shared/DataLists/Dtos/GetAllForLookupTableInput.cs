using Abp.Application.Services.Dto;

namespace ICMSDemo.DataLists.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
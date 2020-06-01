using Abp.Application.Services.Dto;

namespace ICMSDemo.LibraryRisks.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
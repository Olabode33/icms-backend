using Abp.Application.Services.Dto;

namespace ICMSDemo.ExceptionTypeColumns.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
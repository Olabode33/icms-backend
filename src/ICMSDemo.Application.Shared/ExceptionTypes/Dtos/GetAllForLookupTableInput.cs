using Abp.Application.Services.Dto;

namespace ICMSDemo.ExceptionTypes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}
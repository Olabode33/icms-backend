using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.ExceptionTypes.Dtos
{
    public class GetAllExceptionTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }

		public int SeverityFilter { get; set; }



    }
}
using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.LibraryRisks.Dtos
{
    public class GetAllLibraryRisksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string ProcessFilter { get; set; }

		public string SubProcessFilter { get; set; }



    }
}
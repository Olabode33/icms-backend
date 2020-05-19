using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.Processes.Dtos
{
    public class GetAllProcessesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string OrganizationUnitDisplayNameFilter { get; set; }

		 
    }
}
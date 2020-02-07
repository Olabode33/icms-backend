using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.Risks.Dtos
{
    public class GetAllRisksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public int SeverityFilter { get; set; }



    }
}
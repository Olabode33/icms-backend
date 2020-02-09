using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.DepartmentRiskControls.Dtos
{
    public class GetAllDepartmentRiskControlsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int FrequencyFilter { get; set; }

		public long? DepartmentId { get; set; }
		public string DepartmentRiskCodeFilter { get; set; }

		 		 public string ControlCodeFilter { get; set; }

		 
    }
}
using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.DepartmentRisks.Dtos
{
    public class GetAllDepartmentRisksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }


		 public string DepartmentNameFilter { get; set; }

		 		 public string RiskNameFilter { get; set; }

		 
    }
}
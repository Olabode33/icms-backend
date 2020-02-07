using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.DepartmentRisks.Dtos
{
    public class GetAllDepartmentRisksForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }


		 public string DepartmentNameFilter { get; set; }

		 		 public string RiskNameFilter { get; set; }

		 
    }
}
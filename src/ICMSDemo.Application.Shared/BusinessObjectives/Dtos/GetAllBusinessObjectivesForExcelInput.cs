using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.BusinessObjectives.Dtos
{
    public class GetAllBusinessObjectivesForExcelInput
    {
		public string Filter { get; set; }

		public DateTime? MaxStartDateFilter { get; set; }
		public DateTime? MinStartDateFilter { get; set; }

		public DateTime? MaxEndDateFilter { get; set; }
		public DateTime? MinEndDateFilter { get; set; }

		public string ObjectiveTypeFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 
    }
}
using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.ProcessRisks.Dtos
{
    public class GetAllProcessRisksForExcelInput
    {
		public string Filter { get; set; }

		public string CommentsFilter { get; set; }


		 public string OrganizationUnitDisplayNameFilter { get; set; }

		 		 public string RiskNameFilter { get; set; }

		 
    }
}
using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.DepartmentRatingHistory.Dtos
{
    public class GetAllDepartmentRatingHistoryForExcelInput
    {
		public string Filter { get; set; }


		 public string OrganizationUnitDisplayNameFilter { get; set; }

		 		 public string RatingNameFilter { get; set; }

		 
    }
}
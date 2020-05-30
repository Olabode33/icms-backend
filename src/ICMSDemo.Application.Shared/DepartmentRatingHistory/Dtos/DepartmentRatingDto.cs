
using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.DepartmentRatingHistory.Dtos
{
    public class DepartmentRatingDto : EntityDto
    {
		public DateTime RatingDate { get; set; }

		public string ChangeType { get; set; }


		 public long? OrganizationUnitId { get; set; }

		 		 public int? RatingId { get; set; }

		 
    }
}
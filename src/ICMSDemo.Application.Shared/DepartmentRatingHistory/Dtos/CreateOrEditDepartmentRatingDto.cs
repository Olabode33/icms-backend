
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.DepartmentRatingHistory.Dtos
{
    public class CreateOrEditDepartmentRatingDto : EntityDto<int?>
    {

		public DateTime RatingDate { get; set; }
		
		
		public string ChangeType { get; set; }
		
		
		 public long? OrganizationUnitId { get; set; }
		 
		 		 public int? RatingId { get; set; }
		 
		 
    }
}
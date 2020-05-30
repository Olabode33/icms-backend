using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.DepartmentRatingHistory.Dtos
{
    public class GetDepartmentRatingForEditOutput
    {
		public CreateOrEditDepartmentRatingDto DepartmentRating { get; set; }

		public string OrganizationUnitDisplayName { get; set;}

		public string RatingName { get; set;}


    }
}
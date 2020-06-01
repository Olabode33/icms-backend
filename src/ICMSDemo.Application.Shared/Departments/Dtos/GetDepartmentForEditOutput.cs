using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ICMSDemo.DepartmentRatingHistory.Dtos;

namespace ICMSDemo.Departments.Dtos
{
    public class GetDepartmentForEditOutput
    {
		public CreateOrEditDepartmentDto Department { get; set; }

		public string UserName { get; set;}

		public string UserName2 { get; set;}

		public string OrganizationUnitDisplayName { get; set;}
        public string RatingName { get; set; }
        public string RatingCode { get; set; }
        public DepartmentRatingDto[] RatingHistory { get; set; }
    }
}
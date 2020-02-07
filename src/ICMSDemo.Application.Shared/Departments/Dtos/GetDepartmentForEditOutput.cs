using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.Departments.Dtos
{
    public class GetDepartmentForEditOutput
    {
		public CreateOrEditDepartmentDto Department { get; set; }

		public string UserName { get; set;}

		public string UserName2 { get; set;}

		public string OrganizationUnitDisplayName { get; set;}


    }
}
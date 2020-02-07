using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.DepartmentRiskControls.Dtos
{
    public class GetDepartmentRiskControlForEditOutput
    {
		public CreateOrEditDepartmentRiskControlDto DepartmentRiskControl { get; set; }

		public string DepartmentRiskCode { get; set;}

		public string ControlCode { get; set;}


    }
}
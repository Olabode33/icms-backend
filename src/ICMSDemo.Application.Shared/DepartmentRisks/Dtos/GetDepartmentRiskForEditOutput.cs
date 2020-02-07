using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.DepartmentRisks.Dtos
{
    public class GetDepartmentRiskForEditOutput
    {
		public CreateOrEditDepartmentRiskDto DepartmentRisk { get; set; }

		public string DepartmentName { get; set;}

		public string RiskName { get; set;}


    }
}
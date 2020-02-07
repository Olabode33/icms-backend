
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.DepartmentRisks.Dtos
{
    public class CreateOrEditDepartmentRiskDto : EntityDto<int?>
    {

		public string Comments { get; set; }
		
		
		 public int? DepartmentId { get; set; }
		 
		 		 public int? RiskId { get; set; }
		 
		 
    }
}
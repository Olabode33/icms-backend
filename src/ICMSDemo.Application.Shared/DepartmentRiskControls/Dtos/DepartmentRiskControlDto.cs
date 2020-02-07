using ICMSDemo;

using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.DepartmentRiskControls.Dtos
{
    public class DepartmentRiskControlDto : EntityDto
    {
		public string Code { get; set; }

		public string Notes { get; set; }

		public Frequency Frequency { get; set; }


		 public int? DepartmentRiskId { get; set; }

		 		 public int? ControlId { get; set; }

		 
    }
}
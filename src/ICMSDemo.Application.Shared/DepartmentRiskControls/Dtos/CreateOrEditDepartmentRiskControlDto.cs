using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.DepartmentRiskControls.Dtos
{
    public class CreateOrEditDepartmentRiskControlDto : EntityDto<int?>
    {

		public string Code { get; set; }
		
		
		public string Notes { get; set; }

		public virtual long? DepartmentId { get; set; }


		public Frequency Frequency { get; set; }
		
		
		 public int? DepartmentRiskId { get; set; }
		 
		 		 public int? ControlId { get; set; }

		public bool Cascade { set; get; }
		 
		 
    }
}
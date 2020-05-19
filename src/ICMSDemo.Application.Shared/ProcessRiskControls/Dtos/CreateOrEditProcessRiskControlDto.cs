using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.ProcessRiskControls.Dtos
{
    public class CreateOrEditProcessRiskControlDto : EntityDto<int?>
    {

		public string Code { get; set; }
		
		
		public string Notes { get; set; }
		
		
		public Frequency Frequency { get; set; }
		
		
		public bool Cascade { get; set; }
		
		
		 public int? ProcessRiskId { get; set; }
		 
		 		 public long? ProcessId { get; set; }
		 
		 		 public int? ControlId { get; set; }
		 
		 
    }
}
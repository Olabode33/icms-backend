
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.ProcessRisks.Dtos
{
    public class CreateOrEditProcessRiskDto : EntityDto<int?>
    {

		public string Code { get; set; }
		
		
		public string Comments { get; set; }
		
		
		public bool Cascade { get; set; }
		
		
		 public long ProcessId { get; set; }
		 
		 		 public int RiskId { get; set; }
		 
		 
    }
}
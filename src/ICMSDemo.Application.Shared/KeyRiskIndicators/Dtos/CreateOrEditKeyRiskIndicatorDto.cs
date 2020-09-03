
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.KeyRiskIndicators.Dtos
{
    public class CreateOrEditKeyRiskIndicatorDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		
		public string Nature { get; set; }
		
		
		public decimal LowLevel { get; set; }
		
		
		public string LowActionType { get; set; }
		
		
		public decimal MediumLevel { get; set; }
		
		
		public string MediumActionType { get; set; }
		
		
		public decimal HighLevel { get; set; }
		
		
		public string HighActionType { get; set; }
		
		
		 public int? ExceptionTypeId { get; set; }
		 
		 		 public long? UserId { get; set; }
		 
		 
    }
}
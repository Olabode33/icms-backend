using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.TestingTemplates.Dtos
{
    public class CreateOrEditTestingTemplateDto : EntityDto<int?>
    {

		[Required]
		public string Code { get; set; }
		
		
		public string DetailedInstructions { get; set; }
		
		
		[Required]
		public string Title { get; set; }
		
		
		public Frequency Frequency { get; set; }
		
		
		 public int? DepartmentRiskControlId { get; set; }
		 
		 
    }
}
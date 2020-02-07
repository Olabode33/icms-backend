using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.Risks.Dtos
{
    public class CreateOrEditRiskDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		
		public Severity Severity { get; set; }
		
		

    }
}
using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.ExceptionTypes.Dtos
{
    public class CreateOrEditExceptionTypeDto : EntityDto<int?>
    {

		[Required]
		public string Code { get; set; }
		
		
		[Required]
		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		
		public Severity Severity { get; set; }
		
		
		[Range(ExceptionTypeConsts.MinTargetRemediationValue, ExceptionTypeConsts.MaxTargetRemediationValue)]
		public int? TargetRemediation { get; set; }
		
		

    }
}
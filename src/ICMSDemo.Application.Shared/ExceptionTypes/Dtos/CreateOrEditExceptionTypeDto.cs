using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ICMSDemo.ExceptionTypeColumns.Dtos;

namespace ICMSDemo.ExceptionTypes.Dtos
{
    public class CreateOrEditExceptionTypeDto : EntityDto<int?>
    {
		[Required]
		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		
		public Severity Severity { get; set; }
		
		
		[Range(ExceptionTypeConsts.MinTargetRemediationValue, ExceptionTypeConsts.MaxTargetRemediationValue)]
		public int? TargetRemediation { get; set; }
		
		public CreateOrEditExceptionTypeColumnDto[] OtherColumns { set; get; }

		public long[] Escalations { set; get; }

	}


}
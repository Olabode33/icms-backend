using ICMSDemo;

using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.ExceptionTypes.Dtos
{
    public class ExceptionTypeDto : EntityDto
    {
		public string Code { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public Severity Severity { get; set; }

		public int? TargetRemediation { get; set; }



    }
}
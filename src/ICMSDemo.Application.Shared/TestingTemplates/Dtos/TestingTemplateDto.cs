using ICMSDemo;

using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.TestingTemplates.Dtos
{
    public class TestingTemplateDto : EntityDto
    {
		public string Code { get; set; }

		public string DetailedInstructions { get; set; }

		public string Title { get; set; }

		public Frequency Frequency { get; set; }


		 public int? DepartmentRiskControlId { get; set; }

		 
    }
}
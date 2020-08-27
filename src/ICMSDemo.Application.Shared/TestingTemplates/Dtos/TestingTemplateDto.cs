using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.TestingTemplates.Dtos
{
    public class TestingTemplateDto : EntityDto
    {
		public string Code { get; set; }

		public string DetailedInstructions { get; set; }

		public string Title { get; set; }

		public string Frequency { get; set; }
		public int SampleSize { get; set; }
		public int DaysToComplete { get; set; }

		 public int? DepartmentRiskControlId { get; set; }
        public bool IsActive { get; set; }
		public ProjectOwner? ProjectOwner { get; set; }
	}
}
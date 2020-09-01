using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.TestingTemplates.Dtos
{
    public class CreateOrEditTestingTemplateDto : EntityDto<int?>
    {

		public virtual int DaysToComplete { get; set; }

		public string DetailedInstructions { get; set; }
		
		
		[Required]
		public string Title { get; set; }
		
		
		public Frequency Frequency { get; set; }

		public virtual int? ExceptionTypeId { get; set; }


		public int? DepartmentRiskControlId { get; set; }
		 
		 public int? SampleSize { set; get; }

		public ProjectOwner? ProjectOwner { get; set; }

		public CreateorEditTestTemplateDetailsDto[] Attributes { get; set; }

		public CreateorEditTestTemplateDetailsDto templateContent { get; set; }
	}


	public class CreateorEditTestTemplateDetailsDto
	{
		public int TestingTemplateId { get; set; }
		public string TestAttribute { get; set; }
        public int? Id { get; set; }
        public int Weight { get; set; }
		public int? ParentId { get; set; }

	}
}
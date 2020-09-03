using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.TestingTemplates.Dtos
{
    public class GetTestingTemplateForEditOutput
    {
		public CreateOrEditTestingTemplateDto TestingTemplate { get; set; }

		public string DepartmentRiskControlCode { get; set;}
        public string UserName { get; set; }

        public string OrganizationUnitDisplayName { get; set; }


    }
}
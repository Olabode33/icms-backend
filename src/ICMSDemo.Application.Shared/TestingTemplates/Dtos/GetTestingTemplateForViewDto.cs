﻿using ICMSDemo.Controls.Dtos;
using ICMSDemo.Risks.Dtos;
using ICMSDemo.WorkingPaperNews.Dtos;

namespace ICMSDemo.TestingTemplates.Dtos
{
    public class GetTestingTemplateForViewDto
    {
		public TestingTemplateDto TestingTemplate { get; set; }

		public string DepartmentRiskControlCode { get; set;}
        public string AffectedDepartments { get; set; }
        public string Cascade { get; set; }

        public RiskDto Risk { set; get; }

        public ControlDto Control { set; get; }

        public CreateOrEditTestingAttributeDto[] Attributes { set; get; }
        public string ExceptionTypeName { get; set; }
        public string EntityType { get; set; }
        public string OuDisplayName { get; set; }
        public string ProcessDescription { get; set; }
        public string ProcessOwner { get; set; }
        public string ProcessDepartment { get; set; }
        public string ProcessName { get; set; }
    }
}
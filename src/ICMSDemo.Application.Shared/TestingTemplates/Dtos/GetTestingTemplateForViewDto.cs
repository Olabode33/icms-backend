using ICMSDemo.Controls.Dtos;
using ICMSDemo.Risks.Dtos;

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

        public string[] Attributes { set; get; }
        public string ExceptionTypeName { get; set; }
    }
}
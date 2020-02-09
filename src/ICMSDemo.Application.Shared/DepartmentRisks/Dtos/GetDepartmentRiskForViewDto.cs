namespace ICMSDemo.DepartmentRisks.Dtos
{
    public class GetDepartmentRiskForViewDto
    {
		public DepartmentRiskDto DepartmentRisk { get; set; }

		public string DepartmentName { get; set;}

		public string RiskName { get; set;}
        public string Severity { get; set; }
    }
}
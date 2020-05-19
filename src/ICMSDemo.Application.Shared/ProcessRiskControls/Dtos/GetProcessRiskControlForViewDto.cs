namespace ICMSDemo.ProcessRiskControls.Dtos
{
    public class GetProcessRiskControlForViewDto
    {
		public ProcessRiskControlDto ProcessRiskControl { get; set; }
		public string ProcessRiskCode { get; set;}
		public string OrganizationUnitDisplayName { get; set;}
		public string ControlName { get; set;}
		public bool Inherited { get; set; }

	}
}
namespace ICMSDemo.ProcessRisks.Dtos
{
    public class GetProcessRiskForViewDto
    {
		public ProcessRiskDto ProcessRisk { get; set; }
		public string ProcessName { get; set;}
		public string RiskName { get; set;}
        public bool Inherited { get; set; }
        public string Severity { get; set; }
        public string ProcessCode { get; set; }
        public double InherentRiskScore { get; set; }
        public double ResidualRiskScore { get; set; }
    }
}
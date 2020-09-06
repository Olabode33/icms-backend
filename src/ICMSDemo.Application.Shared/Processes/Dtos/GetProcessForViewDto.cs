namespace ICMSDemo.Processes.Dtos
{
    public class GetProcessForViewDto
    {

		public ProcessDto Process { get; set; }

		public string UserName { get; set;}

		public string OrganizationUnitDisplayName { get; set;}

        public int RiskCount { get; set; }

        public int ControlCount { get; set; }

    }
}
namespace ICMSDemo.WorkingPaperNews.Dtos
{
    public class GetWorkingPaperNewForViewDto
    {
		public WorkingPaperNewDto WorkingPaperNew { get; set; }

		public string TestingTemplateCode { get; set;}

		public string OrganizationUnitDisplayName { get; set;}

		public string AssignedTo { get; set;}

		public string CompletedBy { get; set;}

		public double CompletionLevel { get; set; }
		public string OuCode { get; set; }
		public Frequency? Frequency { get; set; }
		public int? SampleSize { get; set; }
		public string TestingTemplateName { get; set; }
        public string ProjectName { get; set; }
        public string ReviewedBy { get; set; }
    }
}
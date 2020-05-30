namespace ICMSDemo.WorkingPaperNews.Dtos
{
    public class GetWorkingPaperNewForViewDto
    {
		public WorkingPaperNewDto WorkingPaperNew { get; set; }

		public string TestingTemplateCode { get; set;}

		public string OrganizationUnitDisplayName { get; set;}

		public string UserName { get; set;}

		public string UserName2 { get; set;}

		public double CompletionLevel { get; set; }
		public string OuCode { get; set; }
		public Frequency? Frequency { get; set; }
		public int? SampleSize { get; set; }
		public string TestingTemplateName { get; set; }
        public string ProjectName { get; set; }
    }
}
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ICMSDemo.WorkingPaperNews.Dtos;

namespace ICMSDemo.Projects.Dtos
{
    public class GetProjectForEditOutput
    {
		public CreateOrEditProjectDto Project { get; set; }

		public string OrganizationUnitDisplayName { get; set;}

		public string OrganizationUnitDisplayName2 { get; set;}
        public int CompletedTaskCount { get; set; }
        public double CompletionLevel { get; set; }
        public int OpenWorkingPapers { get; set; }
        public double OpenTaskPercent { get; set; }
        public int PendingReviews { get; set; }
        public double PendingReviewsPercent { get; set; }
        public int ExceptionsCount { get; set; }
    }
}
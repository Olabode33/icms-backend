using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.WorkingPaperNews.Dtos
{
    public class GetAllWorkingPaperNewsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public DateTime? MaxTaskDateFilter { get; set; }
		public DateTime? MinTaskDateFilter { get; set; }

		public DateTime? MaxDueDateFilter { get; set; }
		public DateTime? MinDueDateFilter { get; set; }

		public int TaskStatusFilter { get; set; }

		public DateTime? MaxCompletionDateFilter { get; set; }
		public DateTime? MinCompletionDateFilter { get; set; }


		 public string TestingTemplateCodeFilter { get; set; }

		 		 public string OrganizationUnitDisplayNameFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 		 public string UserName2Filter { get; set; }

		 
    }
}
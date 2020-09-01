using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.WorkingPaperReviewComments.Dtos
{
    public class GetAllWorkingPaperReviewCommentsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string PriorityFilter { get; set; }

		public int? StatusFilter { get; set; }

		public DateTime? MaxExpectedCompletionDateFilter { get; set; }
		public DateTime? MinExpectedCompletionDateFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string WorkingPaperCodeFilter { get; set; }

		 		 public string UserName2Filter { get; set; }

		 
    }
}
using ICMSDemo;

using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.WorkingPaperReviewComments.Dtos
{
    public class WorkingPaperReviewCommentDto : EntityDto
    {
		public string Title { get; set; }

		public string Priority { get; set; }

		public Status Status { get; set; }

		public Severity Severity { get; set; }

		public DateTime ExpectedCompletionDate { get; set; }


		 public long? AssigneeUserId { get; set; }

		 		 public Guid? WorkingPaperId { get; set; }

		 		 public long? AssignerUserId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
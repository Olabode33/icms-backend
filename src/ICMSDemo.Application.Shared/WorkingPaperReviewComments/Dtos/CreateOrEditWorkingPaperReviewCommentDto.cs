using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.WorkingPaperReviewComments.Dtos
{
    public class CreateOrEditWorkingPaperReviewCommentDto : EntityDto<int?>
    {

		public string Title { get; set; }
		
		
		public string Comments { get; set; }


		public string Response { get; set; }
		public Status Status { get; set; }
		
		
		public Severity Severity { get; set; }
		
		
		public DateTime ExpectedCompletionDate { get; set; }
		
		
		 public long? AssigneeUserId { get; set; }
		 
		 		 public Guid? WorkingPaperId { get; set; }
		 
		 		 public long? AssignerUserId { get; set; }
		 
		 
    }
}
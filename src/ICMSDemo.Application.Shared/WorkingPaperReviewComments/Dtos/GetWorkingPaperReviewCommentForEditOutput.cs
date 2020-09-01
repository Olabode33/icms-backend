using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.WorkingPaperReviewComments.Dtos
{
    public class GetWorkingPaperReviewCommentForEditOutput
    {
		public CreateOrEditWorkingPaperReviewCommentDto WorkingPaperReviewComment { get; set; }

		public string UserName { get; set;}

		public string WorkingPaperCode { get; set;}

		public string UserName2 { get; set;}


    }
}
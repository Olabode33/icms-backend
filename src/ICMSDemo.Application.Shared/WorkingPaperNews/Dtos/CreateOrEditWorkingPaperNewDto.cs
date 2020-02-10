using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.WorkingPaperNews.Dtos
{
    public class CreateOrEditWorkingPaperNewDto : EntityDto<Guid?>
    {

		public string Code { get; set; }
		
		
		public string Comment { get; set; }
		
		
		public DateTime TaskDate { get; set; }
		
		
		public DateTime DueDate { get; set; }
		
		
		public TaskStatus TaskStatus { get; set; }
		
		
		public decimal Score { get; set; }
		
		
		public DateTime ReviewDate { get; set; }
		
		
		public DateTime? CompletionDate { get; set; }
		
		
		 public int? TestingTemplateId { get; set; }
		 
		 		 public long? OrganizationUnitId { get; set; }
		 
		 		 public long? CompletedUserId { get; set; }
		 
		 		 public long? ReviewedUserId { get; set; }
		 
		 
    }
}
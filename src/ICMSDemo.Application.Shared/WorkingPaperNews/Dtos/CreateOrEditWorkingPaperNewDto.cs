using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.WorkingPaperNews.Dtos
{
    public class CreateOrEditWorkingPaperNewDto : EntityDto<Guid?>
    {
		public CreateOrEditTestingAttributeDto[] Attributes { set; get; }
		public string Code { get; set; }
		
		
		public string Comment { get; set; }
		
		
		public DateTime TaskDate { get; set; }
		
		
		public DateTime DueDate { get; set; }
		
		
		public TaskStatus TaskStatus { get; set; }
		
		
		public decimal Score { get; set; }
		
		
		public DateTime? ReviewDate { get; set; }
		
		
		public DateTime? CompletionDate { get; set; }
		
		
		 public int? TestingTemplateId { get; set; }
		 
		 		 public long? OrganizationUnitId { get; set; }
		 
		 		 public long? CompletedUserId { get; set; }
		 
		 		 public long? ReviewedUserId { get; set; }
		 
		 
    }

	public class CreateOrEditTestingAttributeDto {
		public string AttributeText { get; set; }
		public bool Result { get; set; }
		public int Weight { get; set; }
		public string Comments { get; set; }
		public int? TestingAttrributeId { get; set; }
        public int Sequence { get; set; }
		public Guid? WorkingPaperId { get; set; }
    }

}
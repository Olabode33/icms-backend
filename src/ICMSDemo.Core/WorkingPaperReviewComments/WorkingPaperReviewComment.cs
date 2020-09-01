using ICMSDemo;
using ICMSDemo.Authorization.Users;
using ICMSDemo.WorkingPapers;
using ICMSDemo.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ICMSDemo.WorkingPaperReviewComments
{
	[Table("WorkingPaperReviewComments")]
    public class WorkingPaperReviewComment : CreationAuditedEntity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual string Title { get; set; }
		
		public virtual string Comments { get; set; }
		public virtual string Response { get; set; }
		
		
		public virtual DateTime CompletionDate { get; set; }
		
		public virtual Status Status { get; set; }
		
		public virtual Severity Severity { get; set; }
		
		public virtual DateTime ExpectedCompletionDate { get; set; }
		

		public virtual long? AssigneeUserId { get; set; }
		
        [ForeignKey("AssigneeUserId")]
		public User AssigneeUserFk { get; set; }
		
		public virtual Guid? WorkingPaperId { get; set; }
		
        [ForeignKey("WorkingPaperId")]
		public WorkingPaper WorkingPaperFk { get; set; }
		
		public virtual long? AssignerUserId { get; set; }
		
        [ForeignKey("AssignerUserId")]
		public User AssignerUserFk { get; set; }
		
    }
}
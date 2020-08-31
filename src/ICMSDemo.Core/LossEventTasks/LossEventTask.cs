using ICMSDemo;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ICMSDemo.LossEventTasks
{
	[Table("LossEventTasks")]
    public class LossEventTask : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual string Title { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual int? LossTypeId { get; set; }
		
		public virtual int? LossTypeTriggerId { get; set; }
		
		public virtual Status Status { get; set; }
		
		public virtual long? AssignedTo { get; set; }
		
		public virtual DateTime DateAssigned { get; set; }
		

    }
}
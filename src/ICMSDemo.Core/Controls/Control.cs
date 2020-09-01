using ICMSDemo;
using ICMSDemo;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using ICMSDemo.Authorization.Users;

namespace ICMSDemo.Controls
{
	[Table("Controls")]
    [Audited]
    public class Control : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string Code { get; set; }
		
		[Required]
		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual ControlType ControlType { get; set; }
		
		public virtual Frequency Frequency { get; set; }
		
		public virtual string ControlPoint { get; set; }
		public virtual string ControlObjective { get; set; }
		public virtual long? ControlOwnerId { get; set; }

		[ForeignKey("ControlOwnerId")]
		public User ControlOwnerFK { get; set; }
    }
}
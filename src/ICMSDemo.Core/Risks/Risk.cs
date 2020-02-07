using ICMSDemo;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace ICMSDemo.Risks
{
	[Table("Risks")]
    [Audited]
    public class Risk : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string Code { get; set; }
		
		[Required]
		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual Severity Severity { get; set; }
		

    }
}
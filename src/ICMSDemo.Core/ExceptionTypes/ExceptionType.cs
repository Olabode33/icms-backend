using ICMSDemo;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ICMSDemo.ExceptionTypes
{
	[Table("ExceptionTypes")]
    public class ExceptionType : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string Code { get; set; }
		
		[Required]
		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual Severity Severity { get; set; }
		
		[Range(ExceptionTypeConsts.MinTargetRemediationValue, ExceptionTypeConsts.MaxTargetRemediationValue)]
		public virtual int? TargetRemediation { get; set; }

		public virtual ExceptionRemediationTypeEnum? Remediation { get; set; }

    }
}
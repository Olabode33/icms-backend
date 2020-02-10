using ICMSDemo;
using ICMSDemo.ExceptionTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace ICMSDemo.ExceptionTypeColumns
{
	[Table("ExceptionTypeColumns")]
    [Audited]
    public class ExceptionTypeColumn : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual string Name { get; set; }
		
		public virtual DataTypes DataType { get; set; }
		
		public virtual bool Required { get; set; }
		
		public virtual decimal? Minimum { get; set; }
		
		public virtual decimal? Maximum { get; set; }
		

		public virtual int ExceptionTypeId { get; set; }
		
        [ForeignKey("ExceptionTypeId")]
		public ExceptionType ExceptionTypeFk { get; set; }
		
    }

	
}
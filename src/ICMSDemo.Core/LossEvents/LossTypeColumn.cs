using ICMSDemo;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.LossEvents
{
	[Table("LossTypeColumns")]
    public class LossTypeColumn : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
		public virtual string ColumnName { get; set; }
		
		public virtual DataTypes DataType { get; set; }
		
		public virtual bool Required { get; set; }
		
		public virtual int LossTypeId { get; set; }

        [ForeignKey("LossTypeId")]
        public virtual LossType LossTypeFk { get; set; }

        public virtual double? Minimum { get; set; }
		
		public virtual double? Maximum { get; set; }
    }
}
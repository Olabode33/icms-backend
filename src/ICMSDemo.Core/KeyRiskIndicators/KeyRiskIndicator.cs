using ICMSDemo.ExceptionTypes;
using ICMSDemo.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ICMSDemo.Risks;
using ICMSDemo.BusinessObjectives;

namespace ICMSDemo.KeyRiskIndicators
{
	[Table("KeyRiskIndicators")]
    public class KeyRiskIndicator : CreationAuditedEntity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }

		public virtual string DataInputMethod { get; set; }
		public virtual string Nature { get; set; }
		
		public virtual decimal LowLevel { get; set; }
		
		public virtual string LowActionType { get; set; }
		
		public virtual decimal MediumLevel { get; set; }
		
		public virtual string MediumActionType { get; set; }
		
		public virtual decimal HighLevel { get; set; }
		
		public virtual string HighActionType { get; set; }


		public virtual int? RiskId { get; set; }

		[ForeignKey("RiskId")]
		public Risk RiskFk { get; set; }


		public virtual int? BusinessObjectiveId { get; set; }

		[ForeignKey("BusinessObjectiveId")]
		public BusinessObjective BusinessObjectiveFk { get; set; }

		public virtual int? ExceptionTypeId { get; set; }
		
        [ForeignKey("ExceptionTypeId")]
		public ExceptionType ExceptionTypeFk { get; set; }
		
		public virtual long? UserId { get; set; }
		
        [ForeignKey("UserId")]
		public User UserFk { get; set; }
		
    }
}
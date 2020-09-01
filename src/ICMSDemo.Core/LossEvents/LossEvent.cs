using ICMSDemo;
using ICMSDemo.Authorization.Users;
using Abp.Organizations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.LossEvents
{
	[Table("LossEvents")]
    public class LossEvent : FullAuditedEntity , IMayHaveTenant, IExtendableObject
	{
			public int? TenantId { get; set; }
			

		public virtual double Amount { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual DateTime DateOccured { get; set; }
		
		public virtual DateTime DateDiscovered { get; set; }

		public virtual int LossTypeId { get; set; }

		[ForeignKey("LossTypeId")]
		public virtual LossType LossTypeFk { get; set; }

		public virtual Status Status { get; set; }
		
		public virtual LossCategoryEnums LossCategory { get; set; }

		public virtual string ExtensionData { get; set; }

		public virtual long? EmployeeUserId { get; set; }
		
        [ForeignKey("EmployeeUserId")]
		public User EmployeeUserFk { get; set; }
		
		public virtual long? DepartmentId { get; set; }
		
        [ForeignKey("DepartmentId")]
		public OrganizationUnit DepartmentFk { get; set; }
		
    }
}
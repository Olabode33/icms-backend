using ICMSDemo;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ICMSDemo.Authorization.Users;

namespace ICMSDemo.ExceptionTypes
{
	[Table("ExceptionTypeEscalations")]
    public class ExceptionTypeEscalation : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }

		public int ExceptionTypeId { get; set; }
		public ExceptionType ExceptionType { get; set; }
		public long EscalationUserId { get; set; }
		public User EscalationUser { get; set; }






	}
}
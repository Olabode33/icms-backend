using ICMSDemo.Departments;
using ICMSDemo.Risks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ICMSDemo.DepartmentRisks
{
	[Table("DepartmentRisks")]
    public class DepartmentRisk : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual string Code { get; set; }
		
		public virtual string Comments { get; set; }
		

		public virtual long? DepartmentId { get; set; }
		
        [ForeignKey("DepartmentId")]
		public Department DepartmentFk { get; set; }
		
		public virtual int? RiskId { get; set; }
		
        [ForeignKey("RiskId")]
		public Risk RiskFk { get; set; }

		public bool Cascade { set; get; }
		
    }
}
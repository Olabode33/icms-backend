using ICMSDemo;
using ICMSDemo.DepartmentRisks;
using ICMSDemo.Controls;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using ICMSDemo.Departments;

namespace ICMSDemo.DepartmentRiskControls
{
	[Table("DepartmentRiskControls")]
    [Audited]
    public class DepartmentRiskControl : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual string Code { get; set; }
		
		public virtual string Notes { get; set; }
		
		public virtual Frequency Frequency { get; set; }
		
		public virtual int? DepartmentRiskId { get; set; }
		
        [ForeignKey("DepartmentRiskId")]
		public DepartmentRisk DepartmentRiskFk { get; set; }

		public virtual long? DepartmentId { get; set; }

		[ForeignKey("DepartmentId")]
		public Department DepartmentFk { get; set; }

		public virtual int? ControlId { get; set; }
		
        [ForeignKey("ControlId")]
		public Control ControlFk { get; set; }

		public bool Cascade { set; get; }

	}
}
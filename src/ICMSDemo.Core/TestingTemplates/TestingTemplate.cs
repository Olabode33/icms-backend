﻿using ICMSDemo;
using ICMSDemo.DepartmentRiskControls;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;
using ICMSDemo.ExceptionTypes;

namespace ICMSDemo.TestingTemplates
{
	[Table("TestingTemplates")]
    [Audited]
    public class TestingTemplate : FullAuditedEntity , IMustHaveTenant, IExtendableObject
    {
			public int TenantId { get; set; }

		public bool IsActive { get; set; } = true;


		[Required]
		public virtual string Code { get; set; }
		
		public virtual string DetailedInstructions { get; set; }
		
		[Required]
		public virtual string Title { get; set; }
		
		public virtual Frequency Frequency { get; set; }
		

		public virtual int? DepartmentRiskControlId { get; set; }
		
        [ForeignKey("DepartmentRiskControlId")]
		public DepartmentRiskControl DepartmentRiskControlFk { get; set; }
		public string ExtensionData { get; set; }

		public int? SampleSize { set; get; }
	}


	public class TestingAttrribute : CreationAuditedEntity
	{
		public string TestAttribute { get; set; }

		public virtual int? TestingTemplateId { get; set; }

		[ForeignKey("TestingTemplateId")]
		public TestingTemplate TestingTemplate { get; set; }

		public virtual int? ExceptionTypeId { get; set; }

		[ForeignKey("ExceptionTypeId")]
		public ExceptionType ExceptionType { get; set; }

	}
}
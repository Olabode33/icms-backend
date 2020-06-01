using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ICMSDemo.LibraryControls
{
	[Table("LibraryControls")]
    public class LibraryControl : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual string Process { get; set; }
		
		public virtual string SubProcess { get; set; }
		
		public virtual string Risk { get; set; }
		
		public virtual string ControlLevel { get; set; }
		
		public virtual string ControlPoint { get; set; }
		
		public virtual string Frequency { get; set; }
		
		public virtual string InformationProcessingObjectives { get; set; }
		public virtual string ControlType { get; set; }
		

    }
}
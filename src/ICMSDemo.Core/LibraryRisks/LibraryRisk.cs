using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ICMSDemo.LibraryRisks
{
	[Table("LibraryRisks")]
    public class LibraryRisk : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string Name { get; set; }
		
		public virtual string Process { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual string SubProcess { get; set; }
		

    }
}
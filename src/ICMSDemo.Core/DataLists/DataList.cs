using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ICMSDemo.DataLists
{
	[Table("DataLists")]
    public class DataList : Entity , IMustHaveTenant, IExtendableObject
    {
			public int TenantId { get; set; }
			

		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }
		public string ExtensionData { get ; set; }
	}
}
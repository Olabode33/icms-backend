using ICMSDemo.Authorization.Users;
using Abp.Organizations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace ICMSDemo.Processes
{
	[Table("Processes")]
    [Audited]
    public class Process : OrganizationUnit
    {
		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual bool Casade { get; set; }
		

		public virtual long? OwnerId { get; set; }
		
        [ForeignKey("OwnerId")]
		public User OwnerFk { get; set; }
		
		public virtual long? DepartmentId { get; set; }
		
        [ForeignKey("DepartmentId")]
		public OrganizationUnit DepartmentFk { get; set; }
		
    }
}
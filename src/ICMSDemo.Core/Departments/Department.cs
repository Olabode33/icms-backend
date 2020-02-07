using ICMSDemo.Authorization.Users;
using ICMSDemo.Authorization.Users;
using Abp.Organizations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace ICMSDemo.Departments
{
	[Table("Departments")]
    [Audited]
    public class Department : OrganizationUnit 
    {
		public virtual string DepartmentCode { get; set; }
		
		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual bool IsAbstract { get; set; }
		
		public virtual bool IsControlTeam { get; set; }
		

		public virtual long? SupervisorUserId { get; set; }
		
        [ForeignKey("SupervisorUserId")]
		public User SupervisorUserFk { get; set; }
		
		public virtual long? ControlOfficerUserId { get; set; }
		
        [ForeignKey("ControlOfficerUserId")]
		public User ControlOfficerUserFk { get; set; }
		
		public virtual long? ControlTeamId { get; set; }
		
        [ForeignKey("ControlTeamId")]
		public OrganizationUnit ControlTeamFk { get; set; }

		public long? SupervisingUnitId { get; set; }


		[ForeignKey("SupervisingUnitId")]
		public OrganizationUnit SupervisingUnit { get; set; }
	}
}
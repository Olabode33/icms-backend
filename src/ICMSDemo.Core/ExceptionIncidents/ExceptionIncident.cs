using ICMSDemo;
using ICMSDemo.ExceptionTypes;
using ICMSDemo.Authorization.Users;
using ICMSDemo.TestingTemplates;
using Abp.Organizations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ICMSDemo.WorkingPapers;
using ICMSDemo.ExceptionTypeColumns;

namespace ICMSDemo.ExceptionIncidents
{
	[Table("ExceptionIncidents")]
    public class ExceptionIncident : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string Code { get; set; }
		
		public virtual DateTime Date { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual Status Status { get; set; }

		public virtual DateTime? ClosureDate { get; set; }

		public virtual string ClosureComments { get; set; }

		public virtual DateTime? ResolutionDate { get; set; }
		
		public virtual string ResolutionComments { get; set; }
		
		public virtual string RaisedByClosureComments { get; set; }
		

		public virtual int? ExceptionTypeId { get; set; }
		
        [ForeignKey("ExceptionTypeId")]
		public ExceptionType ExceptionTypeFk { get; set; }
		
		public virtual long? RaisedById { get; set; }
		
        [ForeignKey("RaisedById")]
		public User RaisedByFk { get; set; }

		public virtual long? ClosedById { get; set; }

		[ForeignKey("ClosedById")]
		public User ClosedByFk { get; set; }

		public virtual long? CausedById { get; set; }

		[ForeignKey("CausedById")]
		public User CausedByFk { get; set; }

		public virtual Guid? WorkingPaperFkId { get; set; }
		
        [ForeignKey("WorkingPaperFkId")]
		public WorkingPaper WorkingPaperFk { get; set; }
		
		public virtual long? OrganizationUnitId { get; set; }
		
        [ForeignKey("OrganizationUnitId")]
		public OrganizationUnit OrganizationUnitFk { get; set; }
		
    }


	public class ExceptionIncidentColumn : Entity, IMustHaveTenant
	{
		public int TenantId { get; set; }

		public virtual int? ExceptionIncidentId { get; set; }

		[ForeignKey("ExceptionIncidentId")]
		public ExceptionIncident ExceptionIncidentFk { get; set; }

		public virtual int? ExceptionTypeColumnId { get; set; }

		[ForeignKey("ExceptionTypeColumnId")]
		public ExceptionTypeColumn ExceptionTypeColumnFk { get; set; }

		public virtual string Value { get; set; }

	}
}
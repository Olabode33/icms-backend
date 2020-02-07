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
		
		public virtual string RaisedByClosureComments { get; set; }
		

		public virtual int? ExceptionTypeId { get; set; }
		
        [ForeignKey("ExceptionTypeId")]
		public ExceptionType ExceptionTypeFk { get; set; }
		
		public virtual long? RaisedById { get; set; }
		
        [ForeignKey("RaisedById")]
		public User RaisedByFk { get; set; }
		
		public virtual int? TestingTemplateId { get; set; }
		
        [ForeignKey("TestingTemplateId")]
		public TestingTemplate TestingTemplateFk { get; set; }
		
		public virtual long? OrganizationUnitId { get; set; }
		
        [ForeignKey("OrganizationUnitId")]
		public OrganizationUnit OrganizationUnitFk { get; set; }
		
    }
}
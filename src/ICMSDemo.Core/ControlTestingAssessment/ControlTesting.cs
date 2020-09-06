using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ICMSDemo.Authorization.Users;

namespace ICMSDemo.ControlTestingAssessment
{
	[Table("ControlTestings")]
    public class ControlTesting : FullAuditedEntity , IMayHaveTenant
    {
	    public int? TenantId { get; set; }
			
		public virtual string Name { get; set; }
		
		public virtual int? TestingTemplateId { get; set; }
		
		public virtual DateTime? EndDate { get; set; }
        public virtual int? ProcessRiskControlId { get; set; }
        public virtual int? ProjectId { get; set; }
        public int? OrganizationUnitId { get; set; }
        public virtual long? AssignedUserId { get; set; }
        [ForeignKey("AssignedUserId")]
        public User AssignedUserFk { get; set; }
    }
}
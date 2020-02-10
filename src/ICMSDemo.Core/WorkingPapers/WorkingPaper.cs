using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using ICMSDemo.Authorization.Users;
using ICMSDemo.ExceptionIncidents;
using ICMSDemo.TestingTemplates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ICMSDemo.WorkingPapers
{
    public class WorkingPaper : FullAuditedEntity<Guid>, IMustHaveTenant, IMustHaveOrganizationUnit
    {
        public int TenantId { get; set; }
        public long OrganizationUnitId { get; set; }

        public virtual int? TestingTemplateId { get; set; }

        [ForeignKey("TestingTemplateId")]
        public TestingTemplate TestingTemplate { get; set; }

        public long? CompletedById { get; set; }

        public User CompletedBy { get; set; }

        public long? ReviewedById { get; set; }

        public User ReviewedBy { get; set; }

        public DateTime? CompletionDate { get; set; }

        public DateTime? ReviewedDate { get; set; }

        public TaskStatus TaskStatus { get; set; }

        public decimal Score { set; get; }

    }


    public class WorkingPaperDetail : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public int MyProperty { get; set; }
      
        public virtual int? TestingAttrributeId { get; set; }

        [ForeignKey("TestingAttrributeId")]
        public TestingAttrribute TestingAttrribute { get; set; }

        public bool Result { set; get; }

        public string Comments { set; get; }

        public int Sequence { set; get; }

        public string Identifier { set; get; }

        public virtual int? ExceptionIncidentId { get; set; }

        [ForeignKey("ExceptionIncidentId")]
        public ExceptionIncident ExceptionIncident { get; set; }

    }
}

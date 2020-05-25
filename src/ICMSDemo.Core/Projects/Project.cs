using Abp.Organizations;
using Abp.Organizations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace ICMSDemo.Projects
{
	[Table("Projects")]
    [Audited]
    public class Project : Entity , IMustHaveTenant
    {
	    public int TenantId { get; set; }
			

		public virtual string Code { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual DateTime? StartDate { get; set; }
		public virtual DateTime? ScopeStartDate { get; set; }
		public virtual DateTime? ScopeEndDate { get; set; }


		
		public virtual DateTime? EndDate { get; set; }
		
		public virtual DateTime BudgetedStartDate { get; set; }
		
		public virtual DateTime BudgetedEndDate { get; set; }
		
		public virtual string Title { get; set; }
		public virtual ReviewType ReviewType { get; set; }

		public virtual bool Cascade { get; set; }
		public virtual bool Commenced { get; set; } = false;

        public decimal Progress { get; set; }


        public virtual long? ControlUnitId { get; set; }
		
        [ForeignKey("ControlUnitId")]
		public OrganizationUnit ControlUnitFk { get; set; }
		
		public virtual long? ScopeId { get; set; }
		
        [ForeignKey("ScopeId")]
		public OrganizationUnit ScopeFk { get; set; }
		
    }
}
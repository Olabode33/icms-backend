using Abp.Organizations;
using ICMSDemo.Ratings;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ICMSDemo.DepartmentRatingHistory
{
	[Table("DepartmentRatingHistory")]
    public class DepartmentRating : CreationAuditedEntity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual DateTime RatingDate { get; set; }
		
		public virtual string ChangeType { get; set; }
		

		public virtual long? OrganizationUnitId { get; set; }
		
        [ForeignKey("OrganizationUnitId")]
		public OrganizationUnit OrganizationUnitFk { get; set; }
		
		public virtual int? RatingId { get; set; }
		
        [ForeignKey("RatingId")]
		public Rating RatingFk { get; set; }
        public string Comment { get; set; }
    }
}
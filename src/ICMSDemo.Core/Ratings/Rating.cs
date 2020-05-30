using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ICMSDemo.Ratings
{
	[Table("Ratings")]
    public class Rating : FullAuditedEntity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string Name { get; set; }
		
		public virtual string Code { get; set; }
		
		public virtual string Description { get; set; }
		
		
		[Range(RatingConsts.MinUpperBoundaryValue, RatingConsts.MaxUpperBoundaryValue)]
		public virtual decimal UpperBoundary { get; set; }
		

    }
}
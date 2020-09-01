using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ICMSDemo.LossEvents
{
	[Table("LossTypes")]
	public class LossType : Entity, IMayHaveTenant
	{
		public int? TenantId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}

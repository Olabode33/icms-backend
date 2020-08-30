using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ICMSDemo.LossEvents
{
    [Table("LossTypeTriggers")]
    public class LossTypeTrigger : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Frequency Frequency { get; set; }
        public string Staff { get; set; }
        public string Role { get; set; }
        public string DataSource { get; set; }
        public virtual int LossTypeId { get; set; }

        [ForeignKey("LossTypeId")]
        public virtual LossType LossTypeFk { get; set; }
    }
}

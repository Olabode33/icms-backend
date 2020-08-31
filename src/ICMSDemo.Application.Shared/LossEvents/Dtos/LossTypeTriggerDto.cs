using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICMSDemo.LossEvents.Dtos
{
    public class LossTypeTriggerDto: EntityDto
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Frequency Frequency { get; set; }
        public string Staff { get; set; }
        public string Role { get; set; }
        public string DataSource { get; set; }
        public virtual int LossTypeId { get; set; }
        public virtual long? NotifyUserId { get; set; }
    }

    public class GetLossTypeTriggerForView
    {
        public LossTypeTriggerDto LossTypeTrigger { get; set; }
        public string NotifyUserName { get; set; }
    }
}

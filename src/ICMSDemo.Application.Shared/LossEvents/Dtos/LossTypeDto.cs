using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICMSDemo.LossEvents.Dtos
{
    public class LossTypeDto: EntityDto<int?>
    {
        public int? TenantId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}

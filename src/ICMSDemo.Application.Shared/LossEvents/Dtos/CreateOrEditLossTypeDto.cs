using System;
using System.Collections.Generic;
using System.Text;

namespace ICMSDemo.LossEvents.Dtos
{
    public class CreateOrEditLossTypeDto
    {
        public LossTypeDto LossType { get; set; }
        public List<LossTypeColumnDto> LossTypeColumns { get; set; }
        public List<LossTypeTriggerDto> LossTypeTriggers { get; set; }
    }
}

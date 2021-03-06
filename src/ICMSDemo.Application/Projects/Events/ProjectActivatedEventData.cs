﻿
using Abp.Events.Bus;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.Projects.Events
{
    public class ProjectActivatedEventData : EventData
    {
        public int TenantId { get; set; }

        public Project Project { get; set; }

        public ProjectOwner? ProjectOwner { get; set; }
    }
}


using Abp.Events.Bus;

namespace ICMSDemo.Projects.Events
{
    public class ProjectActivatedEventData : EventData
    {
        public int TenantId { get; set; }

        public Project Project { get; set; }
    }
}

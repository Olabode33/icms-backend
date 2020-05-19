using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.ProcessRiskControls.Dtos
{
    public class GetAllProcessRiskControlsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public int FrequencyFilter { get; set; }
        public string ProcessRiskCodeFilter { get; set; }
        public string OrganizationUnitDisplayNameFilter { get; set; }
        public string ControlNameFilter { get; set; }
        public long? ProcessId { get; set; }
    }
}
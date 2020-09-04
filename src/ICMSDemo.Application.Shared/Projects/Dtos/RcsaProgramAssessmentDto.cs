using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICMSDemo.Projects.Dtos
{
    public class RcsaProgramAssessmentDto: EntityDto
    {
        public int TenantId { get; set; }
        public int ProjectId { get; set; }
        public long BusinessUnitId { get; set; }
        public DateTime DateVerified { get; set; }
        public VerificationStatusEnum VerificationStatus { get; set; }
        public long VerifiedByUserId { get; set; }
        public bool Changes { get; set; }
    }

    public class GetRcsaProgramAssessmentForViewDto
    {
        public RcsaProgramAssessmentDto Assessment { get; set; }
        public string UnitHead { get; set; }
        public string VerifiedByUserName { get; set; }
    }

    public class GetAllRcsaProgramAssessmentInput : PagedAndSortedResultRequestDto
    {
        public int ProjectId { get; set; }
    }
}

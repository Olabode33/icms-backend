using Abp.Domain.Entities;
using ICMSDemo.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ICMSDemo.Projects
{
    [Table("RcsaProgramAssessments")]
    public class RcsaProgramAssessment: Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public virtual int ProjectId { get; set; }
        public virtual long BusinessUnitId { get; set; }
        public virtual DateTime? DateVerified { get; set; }
        public virtual VerificationStatusEnum VerificationStatus { get; set; }
        public virtual long? VerifiedByUserId { get; set; }

        [ForeignKey("VerifiedByUserId")]
        public User VerifiedByUserFk { get; set; }
        public virtual bool Changes { get; set; }
    }
}

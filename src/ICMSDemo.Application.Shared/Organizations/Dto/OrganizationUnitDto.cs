using Abp.Application.Services.Dto;

namespace ICMSDemo.Organizations.Dto
{
    public class OrganizationUnitDto : AuditedEntityDto<long>
    {
        public long? ParentId { get; set; }

        public string Code { get; set; }

        public string DisplayName { get; set; }

        public int MemberCount { get; set; }
        
        public int RoleCount { get; set; }

        public string DepartmentCode { get; set; }
        public long? DepartmentId { get; set; }

        public string TestAttribute { get; set; }

        public int Weight { get; set; }

        public virtual int? TestingTemplateId { get; set; }

    }
}
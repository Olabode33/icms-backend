using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.ExceptionIncidents.Dtos
{
    public class GetAllExceptionIncidentsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxDateFilter { get; set; }
        public DateTime? MinDateFilter { get; set; }

        public int StatusFilter { get; set; }

        public DateTime? MaxClosureDateFilter { get; set; }
        public DateTime? MinClosureDateFilter { get; set; }


        public string ExceptionTypeNameFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string TestingTemplateCodeFilter { get; set; }

        public string OrganizationUnitDisplayNameFilter { get; set; }

        public int? ProjectId { get; set; }
        public int? OrganizationUnitId { get; set; }
    }
}
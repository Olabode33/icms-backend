using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.Projects.Dtos
{
    public class GetAllProjectsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public DateTime? MaxBudgetedStartDateFilter { get; set; }
        public DateTime? MinBudgetedStartDateFilter { get; set; }

        public DateTime? MaxBudgetedEndDateFilter { get; set; }
        public DateTime? MinBudgetedEndDateFilter { get; set; }

        public string TitleFilter { get; set; }


        public string OrganizationUnitDisplayNameFilter { get; set; }

        public string OrganizationUnitDisplayName2Filter { get; set; }
        public bool CommencedFilter { get; set; }

    }
}
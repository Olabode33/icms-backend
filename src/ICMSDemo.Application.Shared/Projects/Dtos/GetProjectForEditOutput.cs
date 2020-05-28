using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ICMSDemo.WorkingPaperNews.Dtos;

namespace ICMSDemo.Projects.Dtos
{
    public class GetProjectForEditOutput
    {
		public CreateOrEditProjectDto Project { get; set; }

		public string OrganizationUnitDisplayName { get; set;}

		public string OrganizationUnitDisplayName2 { get; set;}

        public PagedResultDto<GetWorkingPaperNewForViewDto> WorkingPapers { get; set; }
    }
}